using Grpc.Core;
using PsySchedule.GRPC;
using PsySchedule.Interfaces;
using DtoRequest = PsySchedule.Dto.ClientTelegramRequest;
using DtoTelegramData = PsySchedule.Dto.TelegramData;
using GrpcClient = PsySchedule.GRPC;


namespace PsySchedule.Services.Grpc
{
    public class ClientTelegramGrpcService : GrpcClient.ClientTelegramService.ClientTelegramServiceBase
    {
        private readonly IClientCreater _service;

        public ClientTelegramGrpcService(IClientCreater clientTelegramService)
        {
            _service = clientTelegramService;
        }

        public override async Task<GrpcClient.ClientTelegramResponse> CreateClientTelegram( GrpcClient.ClientTelegramRequest request, ServerCallContext context)
        {
            if (request.TelegramData is null)
            {
                throw new RpcException(
                    new Status(
                        StatusCode.InvalidArgument,
                        "TelegramData is required."));
            }

            var telegram = request.TelegramData;

            // Преобразуем gRPC-модель в DTO приложения.
            var dto = new DtoRequest(
                request.FirstName,
                request.HasSecondName ? request.SecondName : null,
                request.HasPhone ? request.Phone : null,
                request.TimeZone,
                new DtoTelegramData(
                    telegram.Id,
                    telegram.HasUserName ? telegram.UserName : null,
                    telegram.HasFirstName ? telegram.FirstName : null,
                    telegram.HasLastName ? telegram.LastName : null,
                    telegram.HasLanguageCode ? telegram.LanguageCode : null,
                    telegram.TelegramChatId
                )
            );

            var result = await _service.CreateAsync(
                dto,
                context.CancellationToken);

            var response = new ClientTelegramResponse
            {
                IsSuccess = result.IsSuccess
            };

            if (result.Error is not null)
            {
                response.Error = new PsySchedule.GRPC.Error
                {
                    ErrorCode = result.Error.ErrorCode,
                    ErrorMessage = result.Error.ErrorMessage
                };
            }

            return response;
        }
    }
}
