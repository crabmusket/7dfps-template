%i = -1;
datablock ShapeBaseImageData(PistolImage) {
   shapeFile = "art/weapons/pistol/pistol.dae";
   mountPoint = 0;

   lightType = "WeaponFireLight";
   lightColor = "0.992126 0.968504 0.708661 1";
   lightRadius = "4";
   lightDuration = "100";
   lightBrightness = 2;

   stateName[%i++] = "ready";
   stateTransitionOnTriggerDown[%i] = "fire";

   stateName[%i++] = "fire";
   stateFire[%i] = true;
   stateScript[%i] = "onFire";
   stateTimeoutValue[%i] = 0.1;
   stateTransitionOnTimeout[%i] = "catch";

   stateName[%i++] = "catch";
   stateScript[%i] = "onCatch";
   stateTransitionOnTriggerUp[%i] = "ready";
};

datablock ItemData(PistolItem) {
   class = Weapon;
   category = "Weapons";
   shapeFile = "art/weapons/pistol/pistol.dae";
   image = PistolImage;
   elasticity = 0.05;
   friction = 0.8;
   mass = 15;
   type = "weapon";
   itemName = "pistol";
};

datablock ProjectileData(PistolProjectile) {
};

function PistolImage::onFire(%this, %obj, %slot) {
   %p = new Projectile() {
      datablock = PistolProjectile;
      initialPosition = %obj.getMuzzlePoint(0);
      initialVelocity = VectorScale(%obj.getMuzzleVector(0), 5);
      sourceObject = %obj;
      sourceSlot = %slot;
   };
   MissionCleanup.add(%p);
}
