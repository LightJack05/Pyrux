using CommunityToolkit.WinUI.UI;
using Pyrux.DataManagement.Restrictions;
using System.Collections.ObjectModel;
using Windows.Graphics.Printing;
using Windows.Networking.NetworkOperators;
using Windows.UI;

namespace Pyrux.Pages;

public sealed partial class ExercisePage
{
    private ObservableCollection<DataManagement.Restrictions.Restriction> _completionRestrictions = new();
    private List<FrameworkElement> _restrictionsCheckElements = new();

    private void ConstructCompletionRestrictionCollection()
    {
        _restrictionsCheckElements.Clear();
        _completionRestrictions.Clear();
        foreach (Restriction restriction in StaticDataStore.ActiveLevel.CompletionRestrictions)
        {
            _completionRestrictions.Add(restriction);
        }
    }

    private void irpRestrictions_ElementPrepared(ItemsRepeater sender, ItemsRepeaterElementPreparedEventArgs args)
    {
        if (args.Element is FrameworkElement element)
        {
            _restrictionsCheckElements.Add(element);
            element.DataContext = _completionRestrictions[args.Index];
            TextBlock textBlock = ((TextBlock)element.FindChildren().ToList()[1]);
            textBlock.Text = ((Restriction)element.DataContext).ToString();
            UpdateRestrictionSatisfaction();
        }
        
    }
    private void UpdateRestrictionSatisfaction()
    {
        foreach (FrameworkElement element in _restrictionsCheckElements)
        {
            SymbolIcon symbolIcon = ((SymbolIcon)element.FindChildren().ToList()[0]);
            Restriction restriction = element.DataContext as Restriction;

            if (restriction.IsRuntimeDependant())
            {
                symbolIcon.Symbol = Symbol.Help;
                symbolIcon.Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 0, 255));
                ToolTipService.SetToolTip(symbolIcon, "Will be determined at runtime");
            }
            else
            {
                symbolIcon.Symbol = restriction.IsSatisfied(ActiveLevel) ? Symbol.Accept : Symbol.Cancel;
                symbolIcon.Foreground = restriction.IsSatisfied(ActiveLevel) ? new SolidColorBrush(Color.FromArgb(255, 0, 255, 0)) : new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
                ToolTipService.SetToolTip(symbolIcon, restriction.IsSatisfied(ActiveLevel) ? "Satisfied" : "Not satisfied");
            }
        }
    }

    
}