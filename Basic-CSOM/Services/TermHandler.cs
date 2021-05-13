using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Taxonomy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basic_CSOM.Entities.Terms
{
    public class TermHandler
    {
        private ClientContext clientContext;
        public TermHandler(ClientContext context)
        {
            clientContext = context;
        }

        public void CreateDepartmentTermSet()
        {
            TaxonomySession taxonomySession = TaxonomySession.GetTaxonomySession(clientContext);
            clientContext.Load(taxonomySession,
                ts => ts.TermStores.Include(
                    store => store.Name,
                    store => store.Groups.Include(
                        group => group.Name
                        )
                    )
                );
            clientContext.ExecuteQuery();

            if (taxonomySession != null)
            {
                TermStore termStore = taxonomySession.GetDefaultSiteCollectionTermStore();
                if (termStore != null)
                {
                    //
                    //  Create group, termset, and terms.
                    //
                    TermGroup myGroup = termStore.CreateGroup("DemoDepartment", Guid.NewGuid());
                    TermSet myTermSet = myGroup.CreateTermSet("DepartmentSet", Guid.NewGuid(), 1033);
                    myTermSet.CreateTerm("HR", 1033, Guid.NewGuid());
                    myTermSet.CreateTerm("Finance", 1033, Guid.NewGuid());
                    myTermSet.CreateTerm("Commercial", 1033, Guid.NewGuid());
                    myTermSet.CreateTerm("Food and Drink", 1033, Guid.NewGuid());
                    myTermSet.CreateTerm("Support", 1033, Guid.NewGuid());
                    var parentIt = myTermSet.CreateTerm("IT", 1033, Guid.NewGuid());
                    parentIt.CreateTerm("IT Test 1", 1033, Guid.NewGuid());
                    parentIt.CreateTerm("IT Test 2", 1033, Guid.NewGuid());
                    clientContext.ExecuteQuery();
                }
            }
        }

        private void DumpTaxonomyItems()
        {
            //
            // Load up the taxonomy item names.
            //
            TaxonomySession taxonomySession = TaxonomySession.GetTaxonomySession(clientContext);
            TermStore termStore = taxonomySession.GetDefaultSiteCollectionTermStore();
            clientContext.Load(termStore,
                    store => store.Name,
                    store => store.Groups.Include(
                        group => group.Name,
                        group => group.TermSets.Include(
                            termSet => termSet.Name,
                            termSet => termSet.Terms.Include(
                                term => term.Name)
                        )
                    )
            );
            clientContext.ExecuteQuery();


            //
            //Writes the taxonomy item names.
            //
            if (taxonomySession != null)
            {
                if (termStore != null)
                {
                    foreach (TermGroup group in termStore.Groups)
                    {
                        Console.WriteLine("Group " + group.Name);

                        foreach (TermSet termSet in group.TermSets)
                        {
                            Console.WriteLine("TermSet " + termSet.Name);

                            foreach (Term term in termSet.Terms)
                            {
                                //Writes root-level terms only.
                                Console.WriteLine("Term " + term.Name);
                            }
                        }
                    }
                }
            }

        }
    }
}
