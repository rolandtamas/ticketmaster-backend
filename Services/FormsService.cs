using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ticketmaster.Models;
using MongoDB.Driver;

namespace ticketmaster.Services
{
    public class FormsService
    {
        private readonly IMongoCollection<ContactForm> _forms;

        public FormsService(IFormsDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _forms = database.GetCollection<ContactForm>(settings.DatabaseCollectionName);
        }

        public List<ContactForm> Get() =>
            _forms.Find(form => true).ToList();
       

        public ContactForm Get(string id) => _forms.Find<ContactForm>(form => form.Id==id).FirstOrDefault();

        public ContactForm Create(ContactForm cf)
        {
            _forms.InsertOne(cf);
            return cf;
        }

        public void Update(string id, ContactForm cfin) => _forms.ReplaceOne(form => form.Id == id, cfin);

        public void Remove(ContactForm cfin) =>
            _forms.DeleteOne(form => form.Id == cfin.Id);

        public void Remove(string id) =>
            _forms.DeleteOne(form => form.Id == id);


    }
}
