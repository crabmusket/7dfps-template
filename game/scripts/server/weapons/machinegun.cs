%i = -1;
datablock ShapeBaseImageData(MachineGunImage) {
   shapeFile = "art/weapons/machinegun/machinegun.dae";
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
   stateTimeoutValue[%i] = 0.05;
   stateTransitionOnTimeout[%i] = "load";
   stateTransitionOnTriggerUp[%i] = "ready";

   stateName[%i++] = "load";
   stateTransitionOnTriggerDown[%i] = "fire";
   stateTransitionOnTriggerUp[%i] = "ready";
};

datablock ItemData(MachineGunItem) {
   class = Weapon;
   category = "Weapons";
   shapeFile = "art/weapons/machinegun/machinegun.dae";
   image = MachineGunImage;
   elasticity = 0.05;
   friction = 0.8;
   mass = 15;
   type = "weapon";
   itemName = "machine gun";
};

datablock ProjectileData(MachineGunProjectile) {
};

function MachineGunImage::onFire(%this, %obj, %slot) {
   %p = new Projectile() {
      datablock = MachineGunProjectile;
      initialPosition = %obj.getMuzzlePoint(0);
      initialVelocity = VectorScale(%obj.getMuzzleVector(0), 5);
      sourceObject = %obj;
      sourceSlot = %slot;
   };
   MissionCleanup.add(%p);
}
