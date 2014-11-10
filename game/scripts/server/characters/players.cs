datablock PlayerData(PlayerSoldier) {
   class = SoldierShape;
   shapeFile = "art/characters/soldier.dae";

   cameraMaxDist = 2;
};

function PlayerSoldier::onAdd(%this, %obj) {
   // Start looking for items to interact with.
   %this.itemSearch(%obj);
}

function PlayerSoldier::onRemove(%this, %obj) {
   cancel(%obj.itemSearch);
}

// Search for items in front of the character. If we see one, we'll notify the
// GameConnection in control of this player. This function runs every 100ms (i.e.
// 10 times/second). The search takes the form of a big sphere in front of the
// player's eye (imagine holding an exercise ball in front of your face).
function PlayerSoldier::itemSearch(%this, %obj) {
   %obj.itemSearch = %this.schedule(100, itemSearch, %obj);
   %radius = 1.2;
   %eye = %obj.getEyePoint();
   %dir = %obj.getEyeVector();
   %pos = VectorAdd(%eye, VectorScale(%dir, %radius));

   InitContainerRadiusSearch(%pos, %radius, $TypeMasks::ItemObjectType);
   %best = 0;
   %bestDot = -1;
   while((%item = ContainerSearchNext()) != 0) {
      %diff = VectorSub(%item.getPosition(), %eye);
      %diffn = VectorNormalize(%diff);
      %dot = VectorDot(%dir, %diffn);
      if (%dot > %bestDot) {
         %bestDot = %dot;
         %best = %item;
      }
   }

   if (isObject(%obj.client)) {
      %obj.client.lookingAtItem(%best);
   }
}

function PlayerSoldier::getInteraction(%this, %obj, %item) {
   %db = %item.getDataBlock();
   switch$ (%db.type) {
      case "pickup":
         %empty = "pick up";
         %full = "swap for";

      case "device":
         %empty = "use";
         %full = "use";
   }

   %msg = (%obj.getMountedImage(0) == 0 ? %empty : %full)
      SPC %db.itemName;

   return %msg;
}
