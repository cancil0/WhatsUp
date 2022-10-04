using Core.Abstract;
using Core.Extension;
using Entities.Abstract;
using Infrastructure.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Core.Concrete
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class BaseController<TService, TBaseEntity, TPrimaryKey> : ControllerBase 
        where TService : class, IGenericDal<TBaseEntity> 
        where TBaseEntity : class
    {
        private readonly IBaseService<TBaseEntity,TService> baseService = Extensions.Resolve<IBaseService<TBaseEntity, TService>>();

        /// <summary>
        /// Get entity with key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Get/{key}")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public ActionResult<TBaseEntity> Get([FromRoute] TPrimaryKey key)
        {
            return Ok(baseService.Get(key));
        }

        /// <summary>
        /// Create with entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Create")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public ActionResult Create([FromBody] TBaseEntity entity)
        {
            baseService.Create(entity);
            return Ok();
        }

        /// <summary>
        /// Update with entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("Update")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public ActionResult Update([FromBody] TBaseEntity entity)
        {
            baseService.Update(entity);
            return Ok();
        }

        /// <summary>
        /// Delete with primary key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("DeleteById/{key}")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public ActionResult Delete([FromRoute] TPrimaryKey key)
        {
            baseService.Delete(key);
            return Ok();
        }

        /// <summary>
        /// Delete with entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("Delete")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public ActionResult Delete([FromBody] TBaseEntity entity)
        {
            baseService.Delete(entity);
            return Ok();
        }
    }
}
