using ApiServer.RabbitMQ;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace ApiServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MQController : ControllerBase
    {
        private readonly RabbitMQClient _rabbitMQClient;

        public MQController(RabbitMQClient rabbitMQClient)
        {
            _rabbitMQClient = rabbitMQClient;
        }

        [HttpGet]
        [Route("sendMsg")]
        [AllowAnonymous]
        public async Task<IActionResult> SendMsg()
        {
            _rabbitMQClient.Send("test", "测试");
            return Ok(await Task.FromResult("消息已发送！"));
        }

        [HttpGet]
        [Route("consumeMsg")]
        [AllowAnonymous]
        public async Task<IActionResult> ConsumeMsg()
        {
            Func<string, bool> func = (x) => {
                Console.WriteLine(x);
                return true;
            };

            var consumer = _rabbitMQClient.Receive("test", func);
            return Ok(await Task.FromResult("消息已消费！"));
        }
    }
}
