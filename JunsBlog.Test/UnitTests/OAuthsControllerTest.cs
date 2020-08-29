using JunsBlog.Controllers;
using JunsBlog.Helpers;
using JunsBlog.Interfaces;
using JunsBlog.Test.Mockups;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Contrib.HttpClient;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace JunsBlog.Test.UnitTests
{
    public class OAuthsControllerTest
    {
        private readonly OAuthsController oAuthController;
        private readonly IJwtTokenHelper jwtTokenHelper;
        public OAuthsControllerTest()
        {
            var logFactory = LoggerFactory.Create(builder => builder.AddConsole());
            var logger = logFactory.CreateLogger<OAuthsController>();
            var httpContextAccessFake = new HttpContextAccessorFake();
            var notificationServiceFake = new NotificationServiceFake();
            var authenticationSettings = new AuthenticationSettingFake();
            var databaseService = new DatabaseServiceFake();
            jwtTokenHelper = new JwtTokenHelper(new JwtSettingsFake());
            oAuthController = new OAuthsController(httpContextAccessFake, authenticationSettings, databaseService, jwtTokenHelper, logger);
        }

        [Fact]
        public async void GoogleSignIn_ValidCode_Success()
        {
            //var mockMessageHandler = new Mock<HttpMessageHandler>();
            //     mockMessageHandler.Protected()
            //    .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            //    .ReturnsAsync(new HttpResponseMessage
            //    {
            //        StatusCode = HttpStatusCode.OK,
            //        Content = new StringContent(testContent)
            //    });


            var handler = new Mock<HttpMessageHandler>();
            var client = handler.CreateClient();

            handler.SetupAnyRequest()
                    .ReturnsResponse(HttpStatusCode.OK);

        }

    }
}
