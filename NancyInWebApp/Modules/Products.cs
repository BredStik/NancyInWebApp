using Nancy;
using Nancy.ModelBinding;
using NHibernate.Criterion;
using DynamicLINQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NancyInWebApp.Modules
{
    public class Products: NancyModule
    {
        private static readonly IList<Product> _products = new List<Product>()
        {
            new Product{ Id = 1, Name = "A", ItemsInStock = 54, Price = 54.25m, Components = new List<Component>{ new Component{Id = 1, Name = "Component1"}}},
            new Product{ Id = 2, Name = "B", ItemsInStock = 11, Price = 0.99m, Components = new List<Component>{ new Component{Id = 2, Name = "Component2"}, new Component{Id = 3, Name = "Component3"}}},
            new Product{ Id = 3, Name = "C", ItemsInStock = 0, Price = 7.85m, Components = new List<Component>{ new Component{Id = 4, Name = "Component4"}}},
            new Product{ Id = 4, Name = "D", ItemsInStock = 24, Price = 33.00m, Components = new List<Component>{ new Component{Id = 5, Name = "Component5"}, new Component{Id = 6, Name = "Component6"}, new Component{Id = 7, Name = "Component7"}}},
            new Product{ Id = 5, Name = "E", ItemsInStock = 124, Price = 21.99m}
        };

        public Products()
            : base("/products")
        {
            Get["/"] = _ =>
            {
                var queryInfo = new { SortField = Request.Query.SortField.Value, SortDirection = Request.Query.SortDirection.Value, PageIndex = Request.Query.PageIndex.Value, PageSize = Request.Query.PageSize.Value };

                var toReturn = _products.AsQueryable();

                if (queryInfo.SortField != null && queryInfo.SortDirection != null)
                {
                    if (queryInfo.SortDirection.Equals("asc", StringComparison.InvariantCultureIgnoreCase))
                        toReturn = DynamicLINQ.DynamicQueryable.DynamicOrderBy(toReturn, x => x[queryInfo.SortField]).Cast<Product>();

                    if (queryInfo.SortDirection.Equals("desc", StringComparison.InvariantCultureIgnoreCase))
                        toReturn = DynamicLINQ.DynamicQueryable.DynamicOrderByDescending(toReturn, x => x[queryInfo.SortField]).Cast<Product>();
                }

                if (queryInfo.PageIndex != null && queryInfo.PageSize != null)
                {
                    var skippedRecord = (int)Convert.ToInt32(queryInfo.PageIndex) * (int)Convert.ToInt32(queryInfo.PageSize);
                    var pageSize = (int)Convert.ToInt32(queryInfo.PageSize);
                    toReturn = toReturn.Skip(skippedRecord).Take(pageSize);
                }

                return Response.AsJson(toReturn);
            };

            Get["/{id}"] = _ => {
                var product = _products.SingleOrDefault(x => x.Id.Equals(_.id));
                if(product == null)
                    return HttpStatusCode.NotFound;
            
                return Response.AsJson(product);
            };
            
            Get["/{id}/{property}"] = _ => {
                var product = _products.SingleOrDefault(x => x.Id.Equals(_.id));

                if(product == null)
                    return HttpStatusCode.NotFound;

                var pi = product.GetType().GetProperty((string)_.property.Value);

                if(pi == null)
                    return HttpStatusCode.NotFound;

                return Response.AsJson(pi.GetValue(product, null));
            };

            Get["/add"] = _ => View["Add"];

            Post["/"] = _ => {
                var newProduct = this.Bind<Product>();

                //create new product
                newProduct.Id = _products.Max(x => x.Id) + 1;

                _products.Add(newProduct);

                return HttpStatusCode.Created;
            };

            Put["/{id}"] = _ =>
            {
                var product = _products.SingleOrDefault(x => x.Id.Equals(_.id));

                if (product == null)
                    return HttpStatusCode.NotFound;

                this.BindTo(product, x => x.Id);

                return HttpStatusCode.OK;
            };

            Get["/delete/{id}"] = _ => View["delete", new { Id = _.id.Value }];

            Delete["/{id}"] = _ =>
            {
                var product = _products.SingleOrDefault(x => x.Id.Equals(_.id));

                if (product == null)
                    return HttpStatusCode.NotFound;

                _products.Remove(product);

                return HttpStatusCode.OK;
            };
        }
    }

    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int ItemsInStock {get;set;}
        public IList<Component> Components { get; set; }
    }

    public class Component
    {
        public int Id {get;set;}
        public string Name {get;set;}
    }
}