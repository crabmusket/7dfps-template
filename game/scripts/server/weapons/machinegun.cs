%i = -1;
datablock ShapeBaseImageData(MachineGunImage) {
   shapeFile = "art/weapons/machinegun/machinegun.dae";
   eyeOffset = "0.5 0.5 -0.5";

   shakeCamera = true;
   camShakeFreq = "0.1 0.1 0.1";
   camShakeAmp = "0.1 0.1 0.1";

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
   stateTransitionOnTimeout[%i] = "fire";
};

datablock ItemData(MachineGunItem) {
   class = Weapon;
   category = "Weapons";
   shapeFile = "art/weapons/machinegun/machinegun.dae";
   image = MachineGunImage;
   elasticity = 0.05;
   mass = 15;
   type = "pickup";
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
