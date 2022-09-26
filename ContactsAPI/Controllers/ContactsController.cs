using ContactsAPI.Data;
using ContactsAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System.Reflection;
using System;
using ContactsAPI.MiddleWares;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Protocols;
using System.Net;
using static System.Net.Mime.MediaTypeNames;

namespace ContactsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactsController : Controller
    {
        private readonly ContactsAPIDbContext dbContext;
        public ContactsController(ContactsAPIDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetContacts()
        {
            return Ok(await dbContext.Contacts.ToListAsync());
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetContact([FromRoute] Guid id)
        {   
            try
            {
                var contact = await dbContext.Contacts.FindAsync(id);
                /*
                //request Body
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync("https://localhost:7229/swagger/index.html");
                response.EnsureSuccessStatusCode();
                var responseBody = await response.Content.ReadAsStringAsync();
                */
                
                ////
                ////using var clientt = new HttpClient();
                ////var content = await client.GetStringAsync("https://localhost:7229/swagger/index.html");
                ////
                ////Console.WriteLine("+++++" + "\n" + content + "\n" + "+++++++");
                ////
                if (contact != null)
                {
                    string sc = Ok(contact).ToString();
                    
                    //LoggingMethods.Logging("httpGet", "Get Responce Body", null, sc);
                    LoggingMethods.Logging("HttpGet", "requestBody", null, Ok(contact).ToString());
                    return Ok(contact);
                }
                //Logging(MethodName, RequestBody, Exeption?, MSG_Error.NotFound?)
                LoggingMethods.Logging("HttpGet", "requestBody", null, NotFound().ToString());
                return NotFound();
            }
            catch
            {
                //WriteLog2(Logger, MethodName, Body);
                //methodName    ->GetMethod-n e
                //Body          ->
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> PostContacts(AddContactRequest addContactRequest)
        {
            try
            {
                var contact = new Contact
                {
                    Id = Guid.NewGuid(),
                    Address = addContactRequest.Address,
                    EMail = addContactRequest.EMail,
                    FullName = addContactRequest.FullName,
                    Phone = addContactRequest.Phone
                };
                await dbContext.Contacts.AddAsync(contact);
                await dbContext.SaveChangesAsync();

                string text = JsonConvert.SerializeObject(contact);

                LoggingMethods.Logging("HttpPost", text, null, Ok().ToString());

                return Ok(contact);
            }
            catch (Exception)
            {
                //TODO
                string txt = "We cannot Post your request";
                LoggingMethods.Logging("HttpPost", txt, null, NotFound().ToString());   
                return NotFound();
            }
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateContact([FromRoute] Guid id, UpdateContactRequest updateContactRequest)
        {
            var contact = await dbContext.Contacts.FindAsync(id);
            if (contact != null)
            {
                contact.FullName = updateContactRequest.FullName;
                contact.Address = updateContactRequest.Address;
                contact.Phone = updateContactRequest.Phone;
                contact.EMail = updateContactRequest.EMail;

                await dbContext.SaveChangesAsync();

                return Ok(contact);
            }
            return NotFound();
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteContact([FromRoute] Guid id)
        {
            var contact = await dbContext.Contacts.FindAsync(id);
            if (contact != null)
            {
                dbContext.Remove(contact);
                await dbContext.SaveChangesAsync();
                return Ok(contact);
            }
            return NotFound();
        }
    }
}
