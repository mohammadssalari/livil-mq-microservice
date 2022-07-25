using RestSharp;

namespace livil_mq_microservice.Models
{
    public class RecievingApiConfig
    {
        public string BaseUrl { get; set; }
        public string Resource { get; set; }

        public string MyMethod { get; set; }

        public Method MyMethodType
        {
            get
            {
                if (!Enum.IsDefined(typeof(Method), MyMethod))
                {
                    return Method.Post;
                }
                else
                {
                    return Enum.Parse<Method>(MyMethod);
                }

            }
        }
    }


}
