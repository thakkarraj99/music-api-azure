using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ArtistData;
namespace finalWebAPI.Controllers
{
    public class ArtistsController : ApiController
    {
        
        public HttpResponseMessage GetArtistByName(string genre = "All",string first_name = "null", string last_name = "null")
        {
            using (artistsDBEntities entities = new artistsDBEntities())
            {
                if (genre.ToLower() == "all" && first_name.ToLower() == "null" && last_name.ToLower() == "null")
                {
                    return Request.CreateResponse(HttpStatusCode.OK, entities.artists.ToList());
                }
                if (genre.ToLower() == "all" && first_name.ToLower() != "null" && last_name.ToLower() == "null")
                {
                    var entity = entities.artists.Where(e => e.FirstName.ToLower() == first_name.ToLower()).ToList();
                    if (entity == null || entity.Count() == 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound, "Artist with the name " + first_name + " is not available");
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, entity);
                        
                    }
                    
                }
                if (genre.ToLower() == "all" && first_name.ToLower() == "null" && last_name.ToLower() != "null")
                {
                    var entity = entities.artists.Where(e => e.LastName.ToLower() == last_name.ToLower()).ToList();
                    if (entity == null || entity.Count() == 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound, "Artist with the name " + last_name + " is not available");
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, entity);
                    }
                    
                }
                if (genre.ToLower() == "all" && first_name.ToLower() != "null" && last_name.ToLower() != "null")
                {
                    var entity = entities.artists.Where(e => e.LastName.ToLower() == last_name.ToLower() && e.FirstName.ToLower() == first_name.ToLower()).ToList();
                    if (entity == null || entity.Count() == 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound, "Artist with the name "+ first_name+" " + last_name + " is not available");
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, entity);
                    }
                    
                }
                //if genre not all
                if (genre.ToLower() != "all" && first_name.ToLower() != "null" && last_name.ToLower() == "null")
                {
                    var entity = entities.artists.Where(e => e.FirstName.ToLower() == first_name.ToLower() && e.Genre.ToLower() == genre.ToLower()).ToList();
                    if (entity == null || entity.Count() == 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound, "Artist with the name " + first_name + " is not available with genre " +genre );
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, entity);
                    }
                    
                }
                if (genre.ToLower() != "all" && first_name.ToLower() == "null" && last_name.ToLower() != "null")
                {
                    var entity = entities.artists.Where(e => e.LastName.ToLower() == last_name.ToLower() && e.Genre.ToLower() == genre.ToLower()).ToList();
                    if (entity == null || entity.Count() == 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound, "Artist with the name " + last_name + " is not available with genre " + genre);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, entity);
                    }
                    
                }
                if (genre.ToLower() != "all" && first_name.ToLower() != "null" && last_name.ToLower() != "null")
                {
                    var entity = entities.artists.Where(e => e.LastName.ToLower() == last_name.ToLower() && e.FirstName.ToLower() == first_name.ToLower() && e.Genre.ToLower() == genre.ToLower()).ToList();
                    if (entity == null || entity.Count() == 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound, "Artist with the name "+ first_name +" " + last_name + " is not available with genre " + genre);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, entity);
                    }
                   
                }
                if (genre.ToLower() != "all" && first_name.ToLower() == "null" && last_name.ToLower() == "null")
                {
                    var entity = entities.artists.Where(e => e.Genre.ToLower() == genre.ToLower()).ToList();
                    if (entity == null || entity.Count() == 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound, "Artists are not available with genre " + genre);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, entity);
                    }
                    
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Genre value " + first_name + " & " + last_name + " is not valid. See the api page for all get request for genre");
                }
            }
        }
        public HttpResponseMessage Get(int id)
        {
            using (artistsDBEntities entities = new artistsDBEntities())
            {
                var entity =  entities.artists.FirstOrDefault(e => e.ID == id);
                if (entity != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, entity);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Artist with the id " + id.ToString() + " is not available");
                }
            }
        }
        public HttpResponseMessage Post([FromUri]artist artist)
        {
            try { 
            using (artistsDBEntities entities = new artistsDBEntities())
            {
                entities.artists.Add(artist);
                entities.SaveChanges();

                var message = Request.CreateResponse(HttpStatusCode.Created, artist);
                message.Headers.Location = new Uri(Request.RequestUri + artist.ID.ToString());
                    return message;
            }
            }
            catch(Exception ex)
            {
              return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                using (artistsDBEntities entities = new artistsDBEntities())
                {
                    var entity = entities.artists.FirstOrDefault(e => e.ID == id);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "The artist with the id " + id.ToString() + "not found to delete");
                    }
                    else
                    {
                        entities.artists.Remove(entity);
                        entities.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }

                }

            }
            catch(Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e);
            }

        }
        public HttpResponseMessage Put(int id, [FromUri]artist artist)
        {
            using (artistsDBEntities entities = new artistsDBEntities())
            {
                try { 
                var entity = entities.artists.FirstOrDefault(e => e.ID == id);
                if (entity == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Artist with Id " + id.ToString() + " not found to update");
                }
                else
                {
                    entity.FirstName = artist.FirstName;
                    entity.LastName = artist.LastName;
                    entity.Genre = artist.Genre;
                    entity.NumberOfAlbums = artist.NumberOfAlbums;
                    entities.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK, entity);
                }
                }
                catch (Exception e)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e);
                }
            }
        }
    }
}
