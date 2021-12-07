﻿namespace Fabulous

open Fabulous

type IAttributeDefinition =
    abstract member Key: AttributeKey
    abstract member UpdateTarget: newValueOpt: obj voption * viewNode: IViewNode * target: obj -> unit

type IScalarAttributeDefinition =
    inherit IAttributeDefinition
    abstract member CompareBoxed: a: obj * b: obj -> ScalarAttributeComparison

/// Attribute definition for scalar properties
type ScalarAttributeDefinition<'inputType, 'modelType> =
    { Key: AttributeKey
      Name: string
      Convert: 'inputType -> 'modelType
      Compare: 'modelType * 'modelType -> ScalarAttributeComparison
      UpdateTarget: 'modelType voption * IViewNode * obj -> unit }

    member x.WithValue(value): ScalarAttribute =
        { Key = x.Key
#if DEBUG
          DebugName = x.Name
#endif
          Value = x.Convert(value) }

    interface IScalarAttributeDefinition with
        member x.Key = x.Key
        member x.CompareBoxed(a, b) = x.Compare (unbox<'modelType> a, unbox<'modelType> b)
        member x.UpdateTarget(newValueOpt, viewNode, target) =
            let newValueOpt = match newValueOpt with ValueNone -> ValueNone | ValueSome v -> ValueSome (unbox<'modelType> v)
            x.UpdateTarget(newValueOpt, viewNode, target)

/// Attribute definition for widget properties
type WidgetAttributeDefinition =
    { Key: AttributeKey
      Name: string
      ApplyDiff: WidgetDiff * obj -> unit
      UpdateTarget: Widget voption * IViewNode * obj -> unit }

    member x.WithValue(value: Widget): WidgetAttribute =
        { Key = x.Key
#if DEBUG
          DebugName = x.Name
#endif
          Value = value }

    interface IAttributeDefinition with
        member x.Key = x.Key
        member x.UpdateTarget(newValueOpt, viewNode, target) =
            let newValueOpt = match newValueOpt with ValueNone -> ValueNone | ValueSome v -> ValueSome (unbox<Widget> v)
            x.UpdateTarget(newValueOpt, viewNode, target)
            
/// Attribute definition for collection properties
type WidgetCollectionAttributeDefinition =
    { Key: AttributeKey
      Name: string
      ApplyDiff: WidgetCollectionItemChange[] * obj -> unit
      UpdateTarget: Widget[] voption * IViewNode * obj -> unit }
            
    member x.WithValue(value: Widget[]): WidgetCollectionAttribute =
        { Key = x.Key
#if DEBUG
          DebugName = x.Name
#endif
          Value = value }
            
    interface IAttributeDefinition with
        member x.Key = x.Key
        member x.UpdateTarget(newValueOpt, viewNode, target) =
            let newValueOpt = match newValueOpt with ValueNone -> ValueNone | ValueSome v -> ValueSome (unbox<Widget[]> v)
            x.UpdateTarget(newValueOpt, viewNode, target)
