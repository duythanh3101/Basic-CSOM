using Microsoft.SharePoint.Client;
using System.Collections.Generic;

namespace Basic_CSOM.Entities.Lists
{
    public class DocProjectList : BaseList
    {
        private ClientContext context;

        public DocProjectList(ClientContext context) : base(context)
        {
            this.context = context;
            Title = "ProjectDocumentList";
            ContentTypeName = "Project Document";
            TemplateType = (int)ListTemplateType.DocumentLibrary;
            ViewTitle = "All Documents";
            ShowColumns = new List<string>
            {
                "DocDescription",
                "DocType"
            };
        }

        public override void UpdateListitemLookup(List list, ListCollection webListCollection)
        {
            FieldCollection fields = list.Fields;
            context.Load(list, l => l.Id);

            CamlQuery camlQueryForItem = new CamlQuery();
            camlQueryForItem.ViewXml = @"<View>
                                            <Query>
                                                <Where>
                                                    <Eq>
                                                        <FieldRef Name='ID'/>
                                                        <Value Type='Counter'>6</Value>
                                                    </Eq>
                                                </Where>
                                            </Query>
                                        </View>";
            ListItemCollection listItems = list.GetItems(camlQueryForItem);
            context.Load(listItems);
            context.ExecuteQuery();
            ListItem itemToUpdate = listItems[0];
            FieldLookupValue lv = itemToUpdate["wlookup"] as FieldLookupValue;
            lv.LookupId = 16;
            itemToUpdate["wlookup"] = lv;
            itemToUpdate.Update();
            context.Load(itemToUpdate);
            context.ExecuteQuery();
        }
    }
}
