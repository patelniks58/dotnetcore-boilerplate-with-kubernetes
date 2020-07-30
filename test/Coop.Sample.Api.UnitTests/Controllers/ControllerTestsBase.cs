using Coop.Sample.Api.Controllers;
using Microsoft.AspNetCore.Http;
using Moq.AutoMock;
using Xunit;

namespace Coop.Sample.Api.UnitTests.Controllers
{
    [Trait("TestType", "Unit")]

    public abstract class ControllerTestsBase<T>
        where T : ApiControllerBase
    {
        protected readonly T Controller;
        protected readonly AutoMocker Mocker;

        protected ControllerTestsBase()
        {
            Mocker = new AutoMocker();

            var httpResponseMock = Mocker.GetMock<HttpResponse>();
            httpResponseMock.Setup(mock => mock.Headers).Returns(new HeaderDictionary());

            var httpRequestMock = Mocker.GetMock<HttpRequest>();

            var httpContextMock = Mocker.GetMock<HttpContext>();
            httpContextMock.Setup(mock => mock.Response).Returns(httpResponseMock.Object);
            httpContextMock.Setup(mock => mock.Request).Returns(httpRequestMock.Object);

            Controller = Mocker.CreateInstance<T>();
            Controller.ControllerContext.HttpContext = httpContextMock.Object;
        }
    }
}
