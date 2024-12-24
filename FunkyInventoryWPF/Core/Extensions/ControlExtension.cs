using System.Windows;

namespace FunkyInventoryWPF.Core.Extensions;

public static class ControlExtension
{
    public static T GetLogicalParent<T>(DependencyObject obj)
        where T : DependencyObject
    {
        DependencyObject parent = obj;
        Type oTargetType = typeof(T);
        do
        {
            parent = LogicalTreeHelper.GetParent(parent);
        }
        while (!(parent == null || parent.GetType() == oTargetType || parent.GetType().IsSubclassOf(oTargetType)));

        return parent as T;
    }
}
