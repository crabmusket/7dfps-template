datablock PlayerData(PlayerSoldier) {
   class = SoldierShape;
   shapeFile = "art/characters/soldier.dae";

   cameraMaxDist = 2;
};

function PlayerSoldier::onAdd(%this, %obj) {
   // Start looking for items to interact with.
   %this.itemSearch(%obj);

   // Start inventory management.
   %obj.activeWeapon = 0;
   %obj.inactiveWeapon = 0;
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

// Called when the player looks at an item. Here we must come up with a string
// that describes the interaction we can have with the item we're looking at.
// For example, the message for a closed door might be "open", whereas for a
// weapon when we're already holding one, the message might be "swap for".
function PlayerSoldier::getInteraction(%this, %obj, %item) {
   %db = %item.getDataBlock();
   switch$ (%db.type) {
      case "weapon":
         %free = "pick up";
         %full = "swap for";

      case "pickup":
         %free = "pick up";
         %full = "pick up";

      case "device":
         %free = "use";
         %full = "use";

      default:
         %free = "use";
         %full = "use";
   }

   %msg = (!%obj.activeWeapon || !%obj.inactiveWeapon ? %free : %full)
      SPC %db.itemName;

   return %msg;
}

function PlayerSoldier::use(%this, %obj, %item) {
   %db = %item.getDataBlock();
   switch$ (%db.type) {
      case "weapon":
         if (%obj.activeWeapon && %obj.inactiveWeapon) {
            %this.throwWeapon(%obj);
         }

         if (!%obj.activeWeapon) {
            %obj.activeWeapon = %db;
            %obj.mountImage(%db.image, 0);
            %item.delete();
         } else if (!%obj.inactiveWeapon) {
            %obj.inactiveWeapon = %db;
            %item.delete();
         }

      case "pickup":

      default:
         %item.use();
   }
}

function PlayerSoldier::throwWeapon(%this, %obj) {
   if (%obj.activeWeapon) {
      %item = new Item() {
         datablock = %obj.activeWeapon;
         position = %obj.getEyePoint();
         static = false;
         rotate = false;
      };
      MissionCleanup.add(%item);
      %item.setVelocity(VectorScale(%obj.getForwardVector(), 3));
      %item.setCollisionTimeout(%obj);
      %obj.activeWeapon = 0;
      %obj.unmountImage(0);
   }
}

function PlayerSoldier::swapWeapon(%this, %obj) {
   %tmp = %obj.inactiveWeapon;
   %obj.inactiveWeapon = %obj.activeWeapon;
   %obj.activeWeapon = %tmp;
   if (%obj.activeWeapon) {
      %obj.mountImage(%obj.activeWeapon.image, 0);
   } else {
      %obj.unmountImage(0);
   }
}
