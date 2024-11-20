using HarmonyLib;
using System;
using System.Collections;
using UnityEngine;

namespace ConnectedResourceMagnetModNS
{
    public class ConnectedResourceMagnetMod : Mod
    {
        public override void Ready()
        {
            WorldManager.instance.GameDataLoader.AddCardToSetCardBag(SetCardBagType.AdvancedBuildingIdea, "blueprint_connected_resource_magnet", 1);
        }
    }

    public class ConnectedResourceMagnet : ResourceMagnet
	{
		public override void UpdateCard()
		{
			base.UpdateCard();
            if (string.IsNullOrEmpty(PullCardId))
			{
				nameOverride = SokLoc.Translate("card_connected_resource_magnet_name");
			}
			if (MyGameCard.HasChild) {
				if (MyGameCard.CardConnectorChildren.Any((CardConnector x) => x.ConnectionType == ConnectionType.Transport && x.CardDirection == CardDirection.output && x.ConnectedNode != null)) {
					MyGameCard.StartTimer(5f, CompleteTransfer, SokLoc.Translate("card_connected_resource_magnet_status"), GetActionId("CompleteTransfer"));
				}
			}
			else
			{
				MyGameCard.CancelTimer(GetActionId("CompleteTransfer"));
			}
		}

		public override bool CanHaveCardsWhileHasStatus()
		{
			return true;
		}
	
		[TimedAction("complete_transfer")]
		public void CompleteTransfer()
		{
			if (MyGameCard.CardConnectorChildren.Any((CardConnector x) => x.ConnectionType == ConnectionType.Transport && x.CardDirection == CardDirection.output && x.ConnectedNode != null)) {
				GameCard child  = MyGameCard.GetLeafCard();
			    child.RemoveFromParent();
    			WorldManager.instance.TrySendWithPipe(child, MyGameCard, 0);
            }        
		}		
	}
}