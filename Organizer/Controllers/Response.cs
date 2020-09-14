using System;

namespace Organizer.Controllers
{
    public class Response
    {
        public Response()
        {

        }

        public Response(string date, string title)
        {
            this.date = date;
            this.title = title;
        }

        public string date { get; set; }
        public string title { get; set; }
    }
}
