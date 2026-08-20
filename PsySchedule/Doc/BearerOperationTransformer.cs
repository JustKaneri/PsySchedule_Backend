using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace PsySchedule.Doc
{
    public sealed class BearerOperationTransformer
    : IOpenApiOperationTransformer
    {
        public Task TransformAsync(
            OpenApiOperation operation,
            OpenApiOperationTransformerContext context,
            CancellationToken cancellationToken)
        {
            var metadata =
                       context.Description.ActionDescriptor.EndpointMetadata;

            // [AllowAnonymous] имеет приоритет
            if (metadata.OfType<IAllowAnonymous>().Any())
                return Task.CompletedTask;

            // Ищем [Authorize]
            if (!metadata.OfType<IAuthorizeData>().Any())
                return Task.CompletedTask;

            operation.Security =
            [
                new OpenApiSecurityRequirement
            {
                [
                    new OpenApiSecuritySchemeReference(
                        "Bearer",
                        context.Document)
                ] = []
            }
            ];

            return Task.CompletedTask;
        }
    }
}
