using Business.Queries.GetProducts;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class ProductsController : BaseController
    {

        /// <summary>
        /// Retrieves products 
        /// </summary>
        /// <remarks>
        /// Retrieves all products if no query parameters are specified
        /// <br /> <br /> 
        /// If query parameters are specified then filtered subset of products will be returned
        /// <br /> <br /> 
        /// Response contains filter object containing information about max, min product price,
        /// encountered sizes and array of size 10 for most common words (does not include top 5)
        /// </remarks>
        /// <param name="maxPrice">Defined upper limit for price filtering by price</param>
        /// <param name="size">Filters all products that contain this size</param>
        /// <param name="highlight">Words that are highlighted (wrapped with em html tag)</param>
        /// <param name="cancellationToken"></param>
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] float? maxPrice = null, [FromQuery] string size = null, [FromQuery] string highlight = null, CancellationToken cancellationToken = default)
        {

            #region Caching layer - not needed currently according to business needs but it is a improvements point.
            //var cacheKey = $"GetProductsQuery{maxPrice}{size}{highlight}";

            //var response = await Cache.GetOrAddAsync(
            //    cacheKey,
            //    entry => Mediator.Send(new GetProductsQuery(maxPrice, size, highlight), cancellationToken)
            //);

            //return Ok(response);
            #endregion

            return Ok(await Mediator.Send(new GetProductsQuery(maxPrice, size, highlight), cancellationToken));
        }

    }
}
