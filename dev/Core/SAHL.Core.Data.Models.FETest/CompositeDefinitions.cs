using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models.FETest
{
    [Serializable]
    public partial class CompositeDefinitionsDataModel :  IDataModel, IDataModelWithIdentitySeed, IDataModelWithPrimaryKeyId 
    {
        public CompositeDefinitionsDataModel(int compositeKey, string description, int compositeTransitions, string transitionGroup, string transitionDefinition, bool dateIndicator, int sequence)
        {
            this.CompositeKey = compositeKey;
            this.Description = description;
            this.CompositeTransitions = compositeTransitions;
            this.TransitionGroup = transitionGroup;
            this.TransitionDefinition = transitionDefinition;
            this.DateIndicator = dateIndicator;
            this.Sequence = sequence;
		
        }
		[JsonConstructor]
        public CompositeDefinitionsDataModel(int id, int compositeKey, string description, int compositeTransitions, string transitionGroup, string transitionDefinition, bool dateIndicator, int sequence)
        {
            this.Id = id;
            this.CompositeKey = compositeKey;
            this.Description = description;
            this.CompositeTransitions = compositeTransitions;
            this.TransitionGroup = transitionGroup;
            this.TransitionDefinition = transitionDefinition;
            this.DateIndicator = dateIndicator;
            this.Sequence = sequence;
		
        }		

        public int Id { get; set; }

        public int CompositeKey { get; set; }

        public string Description { get; set; }

        public int CompositeTransitions { get; set; }

        public string TransitionGroup { get; set; }

        public string TransitionDefinition { get; set; }

        public bool DateIndicator { get; set; }

        public int Sequence { get; set; }

        public void SetKey(int key)
        {
            this.Id =  key;
        }
    }
}