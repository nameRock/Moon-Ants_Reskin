
public class ItemFinder : Finder<Element, Item>
{
    protected override Item GetTarget(Element element)
    {
        return element.Item;
    }
}
