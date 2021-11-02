using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using System.Web.Http;
using LynxMobileData;
using LynxWebApi.Commands;
using LynxWebApi.Models;
using LynxWebApi.Utility;
using NLog;

namespace LynxWebApi.Controllers
{
    public class SendDecodersController : BaseController
    {
        private static Logger _messageLog = Log.GetMsgLogger();
        public async Task<int> Post(List<Decoder> decoders)
        {
            //ids field from command table will be overloaded to contain
            //serialized decoders
            _messageLog.Debug(await Request.Content.ReadAsStringAsync());
            var ids = Decoder.SerializeDecoders(decoders);
            _messageLog.Debug("SendDecoder sending decoders:" + ids);
            var command = new Command()
            {
                CommandFamily = (int)CommandFamily.Decoder,
                CommandType = (int)DecoderCommand.DecoderCommandType.Send,
                Ids = ids
            };
            DecoderCommand decoderCommand = new DecoderCommand(command, requestIdentity);
            return decoderCommand.Queue();
        }
        //public async Task Post()
        //{
        //    _messageLog.Debug(await Request.Content.ReadAsStringAsync());
        //}
      
    }
}
