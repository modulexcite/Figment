﻿module FSharpMvc.Result

open System.Linq
open System.Web.Mvc
open System.Web.Routing

let empty = EmptyResult() :> ActionResult

let view viewName model = 
    let viewData = ViewDataDictionary(Model = model)
    ViewResult(ViewData = viewData, ViewName = viewName) :> ActionResult

let content s = 
    ContentResult(Content = s) :> ActionResult

let redirect url =
    RedirectResult(url) :> ActionResult

let redirectToRoute (routeValues: RouteValueDictionary) =
    RedirectToRouteResult(routeValues)

/// doesn't work, can't compare IRouteHandlers
let redirectToAction action =    
    let routes = Enumerable.OfType<Route> RouteTable.Routes
    //let routes = RouteTable.Routes |> Enumerable.OfType
    let handler = FSharpMvcRouteHandler(action) :> IRouteHandler
    let route = routes |> Seq.find (fun r -> r.RouteHandler = handler)
    redirect route.Url

let unauthorized = HttpUnauthorizedResult() :> ActionResult

let status code =
    {new ActionResult() with
        override x.ExecuteResult ctx =
            ctx.HttpContext.Response.StatusCode <- code }