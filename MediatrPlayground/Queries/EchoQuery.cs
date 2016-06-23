using MediatR;

namespace MediatrPlayground.Queries
{
    public class EchoQuery : IRequest<string>, ICacheableRequest
    {
        public string TextToEcho { get; set; }
    }

    public class EchoQueryHandler : IRequestHandler<EchoQuery, string>
    {
        public string Handle(EchoQuery request)
        {
            return request.TextToEcho;
        }
    }

    public interface ICacheableRequest
    {
    }
}