namespace Fabulous.XamarinForms

open System.Runtime.CompilerServices
open Fabulous
open Xamarin.Forms

type IApplication =
    inherit IElement

module Application =
    let WidgetKey = Widgets.register<Application>()

    let MainPage =
        Attributes.definePropertyWidget
            "Application_MainPage"
            (fun target -> ViewNode.get (target :?> Application).MainPage)
            (fun target value -> (target :?> Application).MainPage <- value)

    let Resources =
        Attributes.defineSimpleScalarWithEquality<ResourceDictionary>
            "Application_Resources"
            (fun _ newValueOpt node ->
                let application = node.Target :?> Application

                let value =
                    match newValueOpt with
                    | ValueNone -> application.Resources
                    | ValueSome v -> v

                application.Resources <- value)

    let UserAppTheme =
        Attributes.defineEnum<OSAppTheme>
            "Application_UserAppTheme"
            (fun _ newValueOpt node ->
                let application = node.Target :?> Application

                let value =
                    match newValueOpt with
                    | ValueNone -> OSAppTheme.Unspecified
                    | ValueSome v -> v

                application.UserAppTheme <- value)

    let RequestedThemeChanged =
        Attributes.defineEvent<AppThemeChangedEventArgs>
            "Application_RequestedThemeChanged"
            (fun target -> (target :?> Application).RequestedThemeChanged)

    let ModalPopped =
        Attributes.defineEvent<ModalPoppedEventArgs>
            "Application_ModalPopped"
            (fun target -> (target :?> Application).ModalPopped)

    let ModalPopping =
        Attributes.defineEvent<ModalPoppingEventArgs>
            "Application_ModalPopping"
            (fun target -> (target :?> Application).ModalPopping)

    let ModalPushed =
        Attributes.defineEvent<ModalPushedEventArgs>
            "Application_ModalPushed"
            (fun target -> (target :?> Application).ModalPushed)

    let ModalPushing =
        Attributes.defineEvent<ModalPushingEventArgs>
            "Application_ModalPushing"
            (fun target -> (target :?> Application).ModalPushing)

[<AutoOpen>]
module ApplicationBuilders =
    type Fabulous.XamarinForms.View with
        static member inline Application<'msg, 'marker when 'marker :> IPage>(mainPage: WidgetBuilder<'msg, 'marker>) =
            WidgetHelpers.buildWidgets<'msg, IApplication>
                Application.WidgetKey
                [| Application.MainPage.WithValue(mainPage.Compile()) |]

[<Extension>]
type ApplicationModifiers =
    [<Extension>]
    static member inline userAppTheme(this: WidgetBuilder<'msg, #IApplication>, value: OSAppTheme) =
        this.AddScalar(Application.UserAppTheme.WithValue(value))

    [<Extension>]
    static member inline resources(this: WidgetBuilder<'msg, #IApplication>, value: ResourceDictionary) =
        this.AddScalar(Application.Resources.WithValue(value))

    [<Extension>]
    static member inline onRequestedThemeChanged
        (
            this: WidgetBuilder<'msg, #IApplication>,
            onRequestedThemeChanged: OSAppTheme -> 'msg
        ) =
        this.AddScalar(
            Application.RequestedThemeChanged.WithValue(fun args -> onRequestedThemeChanged args.RequestedTheme |> box)
        )

    [<Extension>]
    static member inline onModalPopped
        (
            this: WidgetBuilder<'msg, #IApplication>,
            onModalPopped: ModalPoppedEventArgs -> 'msg
        ) =
        this.AddScalar(Application.ModalPopped.WithValue(onModalPopped >> box))

    [<Extension>]
    static member inline onModalPopping
        (
            this: WidgetBuilder<'msg, #IApplication>,
            onModalPopping: ModalPoppingEventArgs -> 'msg
        ) =
        this.AddScalar(Application.ModalPopping.WithValue(onModalPopping >> box))

    [<Extension>]
    static member inline onModalPushed
        (
            this: WidgetBuilder<'msg, #IApplication>,
            onModalPushed: ModalPushedEventArgs -> 'msg
        ) =
        this.AddScalar(Application.ModalPushed.WithValue(onModalPushed >> box))

    [<Extension>]
    static member inline onModalPushing
        (
            this: WidgetBuilder<'msg, #IApplication>,
            onModalPushing: ModalPushingEventArgs -> 'msg
        ) =
        this.AddScalar(Application.ModalPushing.WithValue(onModalPushing >> box))

    /// <summary>Link a ViewRef to access the direct Application instance</summary>
    [<Extension>]
    static member inline reference(this: WidgetBuilder<'msg, IApplication>, value: ViewRef<Application>) =
        this.AddScalar(ViewRefAttributes.ViewRef.WithValue(value.Unbox))
