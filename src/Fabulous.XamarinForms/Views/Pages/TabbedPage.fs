namespace Fabulous.XamarinForms

open System.Runtime.CompilerServices
open Fabulous
open Xamarin.Forms

type ITabbedPage =
    inherit IMultiPageOfPage

module TabbedPage =
    let WidgetKey = Widgets.register<TabbedPage> ()

    let BarBackgroundColor =
        Attributes.defineAppThemeBindable<Color> TabbedPage.BarBackgroundColorProperty

    let BarTextColor =
        Attributes.defineAppThemeBindable<Color> TabbedPage.BarTextColorProperty

    let SelectedTabColor =
        Attributes.defineAppThemeBindable<Color> TabbedPage.SelectedTabColorProperty

    let UnselectedTabColor =
        Attributes.defineAppThemeBindable<Color> TabbedPage.UnselectedTabColorProperty

[<AutoOpen>]
module TabbedPageBuilders =
    type Fabulous.XamarinForms.View with
        static member inline TabbedPage<'msg>(title: string) =
            CollectionBuilder<'msg, ITabbedPage, IPage>(
                TabbedPage.WidgetKey,
                MultiPageOfPage.Children,
                Page.Title.WithValue(title)
            )

[<Extension>]
type TabbedPageModifiers =

    /// <summary>Set the color of the bar background</summary>
    /// <param name="light">The color of the bar background in the light theme.</param>
    /// <param name="dark">The color of the bar background in the dark theme.</param>
    [<Extension>]
    static member inline barBackgroundColor(this: WidgetBuilder<'msg, #ITabbedPage>, light: Color, ?dark: Color) =
        this.AddScalar(TabbedPage.BarBackgroundColor.WithValue(AppTheme.create light dark))

    /// <summary>Set the color of the bar text</summary>
    /// <param name="light">The color of the bar text in the light theme.</param>
    /// <param name="dark">The color of the bar text in the dark theme.</param>
    [<Extension>]
    static member inline barTextColor(this: WidgetBuilder<'msg, #ITabbedPage>, light: Color, ?dark: Color) =
        this.AddScalar(TabbedPage.BarTextColor.WithValue(AppTheme.create light dark))

    /// <summary>Set the color of the selected tab</summary>
    /// <param name="light">The color of the selected tab in the light theme.</param>
    /// <param name="dark">The color of the selected tab in the dark theme.</param>
    [<Extension>]
    static member inline selectedTabColor(this: WidgetBuilder<'msg, #ITabbedPage>, light: Color, ?dark: Color) =
        this.AddScalar(TabbedPage.SelectedTabColor.WithValue(AppTheme.create light dark))

    /// <summary>Set the color of the unselected tab</summary>
    /// <param name="light">The color of the unselected tab in the light theme.</param>
    /// <param name="dark">The color of the unselected tab in the dark theme.</param>
    [<Extension>]
    static member inline unselectedTabColor(this: WidgetBuilder<'msg, #ITabbedPage>, light: Color, ?dark: Color) =
        this.AddScalar(TabbedPage.UnselectedTabColor.WithValue(AppTheme.create light dark))

    /// <summary>Link a ViewRef to access the direct TabbedPage control instance</summary>
    [<Extension>]
    static member inline reference(this: WidgetBuilder<'msg, ITabbedPage>, value: ViewRef<TabbedPage>) =
        this.AddScalar(ViewRefAttributes.ViewRef.WithValue(value.Unbox))
