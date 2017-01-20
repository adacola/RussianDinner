namespace Adacola.RussianDinner.Meal

open Adacola.RussianDinner.Model
open System

[<AutoOpen>]
module Interface =
    type MealsProvider = unit -> seq<Meal>

module Caloriecalorie =
    open FSharp.Data
    open System.Text.RegularExpressions

    let uri = Uri "http://caloriecalorie.web.fc2.com/"
    let content = lazy(Http.RequestString(uri.AbsoluteUri, responseEncodingOverride = "UTF-8"))

    let parseContent content =
        let html = HtmlDocument.Parse content
        let table = html.CssSelect("div.content table") |> List.find (HtmlNode.hasAttribute "summary" "食品・食事・食べ物のカロリー・カロリー表")
        let meals =
            table.CssSelect("tr") |> Seq.choose (fun row ->
                match row.CssSelect("td") with
                | first::meal::_ when first |> HtmlNode.tryGetAttribute "colspan" |> Option.isNone -> meal |> HtmlNode.innerText |> Some
                | _ -> None)
        let normalize meal =
            let meal = Regex.Replace(meal, @"(?:\(|（).*?(?:\)|）)+\z", "")
            let meal = Regex.Replace(meal, @"\r\n|\n|\r", "")
            meal.Trim(' ', '　')
        meals |> Seq.map normalize |> Seq.distinct

    let provideMeals() = content.Value |> parseContent
