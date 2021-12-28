using CyberSaloon.Core.DTO.Common;
using CyberSaloon.Core.Repo.Common;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CyberSaloon.Core.API.Common
{
    public abstract class CyberSaloonController : ControllerBase
    {
        protected void IntroduceMetadata<T>(
                string controllerName,
                ResourceParameters parameters, 
                Pagination<T> entities
            )
        {
            var metadata =
                new
                {
                    previousPage =
                        CreateGetApplicantsResourceUri<T>(
                            controllerName,
                            entities,
                            parameters,
                            ResourceType.PreviuousPage
                        ),
                    nextPage =
                        CreateGetApplicantsResourceUri(
                            controllerName,
                            entities,
                            parameters,
                            ResourceType.NextPage
                        ),
                };

            Response.Headers.Add(
                    "X-Pagination",
                    JsonConvert.SerializeObject(
                            metadata
                        )
                );
        }

        private string CreateGetApplicantsResourceUri<T>(
                string controllerName,
                Pagination<T> entities,
                ResourceParameters parameters,
                ResourceType type
            )
        {
            switch (type)
            {
                case(ResourceType.PreviuousPage):
                    return
                        parameters.PageNumber > 1 ?
                        Url.Link(
                            controllerName,
                            new
                            {
                                pageNumber = 
                                    entities.TotalPages > parameters.PageNumber ? 
                                        parameters.PageNumber - 1 : 
                                        entities.TotalPages,
                                pageSize = parameters.PageSize
                            }
                        ) : 
                        string.Empty;
                case(ResourceType.NextPage):
                default:
                    return
                        parameters.PageNumber < entities.TotalPages ?
                        Url.Link(
                            controllerName,
                            new
                            {
                                pageNumber = 
                                    entities.TotalPages <= parameters.PageNumber ? 
                                        entities.TotalPages :
                                        parameters.PageNumber + 1,
                                pageSize = parameters.PageSize
                            }
                        ) : 
                        string.Empty;
            }
        }
    }
}