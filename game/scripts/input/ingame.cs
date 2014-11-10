// Metaprogramming!
function setGlobal(%var) {
   %funcName = "_set_"@%var;
   eval("function" SPC %funcName @ "(%val) { $"@%var@" = %val;}");
   return %funcName;
}

//-----------------------------------------------------------------------------
// Generic in-game controls.
new ActionMap(InGameMap);

InGameMap.bindCmd(keyboard, escape, %escape, "");
InGameMap.bindCmd(gamepad, btn_back, %escape, "");
InGameMap.bind(keyboard, f11, toggleEditor);

InGameMap.bindCmd(keyboard, v, "ServerConnection.setFirstPerson(!ServerConnection.isFirstPerson());", "");
InGameMap.bindCmd(gamepad, btn_rt, "ServerConnection.setFirstPerson(!ServerConnection.isFirstPerson());", "");

//-----------------------------------------------------------------------------
// Controlling a character
new ActionMap(CharacterMap);

// Movement
function CharacterMap::yaw(%this, %amount) {
   $mvYaw += %amount * 0.01;
}

function CharacterMap::pitch(%this, %amount) {
   $mvPitch += %amount * 0.01;
}

CharacterMap.bindObj(mouse, "xaxis", yaw, CharacterMap);
CharacterMap.bindObj(mouse, "yaxis", pitch, CharacterMap);

function CharacterMap::gpYaw(%this, %amount) {
   if (%amount > 0) {
      $mvYawLeftSpeed = %amount * 0.25;
      $mvYawRightSpeed = 0;
   } else {
      $mvYawLeftSpeed = %amount * 0.25;
      $mvYawRightSpeed = 0;
   }
}

function CharacterMap::gpPitch(%this, %amount) {
   if (%amount > 0) {
      $mvPitchDownSpeed = %amount * 0.25;
      $mvPitchUpSpeed = 0;
   } else {
      $mvPitchDownSpeed = %amount * 0.25;
      $mvPitchUpSpeed = 0;
   }
}

CharacterMap.bindObj(gamepad, thumbrx, "D", "-0.23 0.23", gpYaw, CharacterMap);
CharacterMap.bindObj(gamepad, thumbry, "D", "-0.23 0.23", gpPitch, CharacterMap);

CharacterMap.bind(keyboard, w, setGlobal(mvForwardAction));
CharacterMap.bind(keyboard, s, setGlobal(mvBackwardAction));
CharacterMap.bind(keyboard, a, setGlobal(mvLeftAction));
CharacterMap.bind(keyboard, d, setGlobal(mvRightAction));

CharacterMap.bind(gamepad, thumblx, "D", "-0.23 0.23", setGlobal(mvRightAction));
CharacterMap.bind(gamepad, thumbly, "D", "-0.23 0.23", setGlobal(mvForwardAction));

// Triggers
// 0 - primary fire
// 1 - alt fire (melee)
// 2 - jump
// 3 - crouch
// 4 - aim!
// 5 - sprint
CharacterMap.bindCmd(keyboard, space, "$mvTriggerCount2++;", "$mvTriggerCount2++;");
CharacterMap.bindCmd(gamepad, btn_a, "$mvTriggerCount2++;", "$mvTriggerCount2++;");

/*
function doCrouch(%val)
{
   $mvTriggerCount3++;
}

moveMap.bind(keyboard, lcontrol, doCrouch);
moveMap.bind(gamepad, btn_b, doCrouch);

function doSprint(%val)
{
   $mvTriggerCount5++;
}

moveMap.bind(keyboard, lshift, doSprint);
*/

// Combat
CharacterMap.bindCmd(mouse, button0, "$mvTriggerCount0++;", "$mvTriggerCount0++;");
CharacterMap.bindCmd(keyboard, f, "$mvTriggerCount1++;", "$mvTriggerCount1++;");
CharacterMap.bindCmd(gamepad, btn_b, "$mvTriggerCount1++;", "$mvTriggerCount1++;");

CharacterMap.gpTrigger0Latch = false;
CharacterMap.gpTrigger0High = 0.1;
function CharacterMap::gpTrigger0(%this, %val) {
   if (!%this.gpTrigger0Latch && %val >= %this.gpTrigger0High) {
      %this.gpTrigger0Latch = true;
      $mvTriggerCount0++;
   }

   if (%this.gpTrigger0Latch && %val < %this.gpTrigger0High) {
      %this.gpTrigger0Latch = false;
      $mvTriggerCount0++;
   }
}

CharacterMap.bindObj(gamepad, triggerr, gpTrigger0, CharacterMap);

// Aiming
CharacterMap.bindCmd(mouse, button1, "$mvTriggerCount4++;", "$mvTriggerCount4++;");

CharacterMap.gpTrigger1Latch = false;
CharacterMap.gpTrigger1High = 0.1;
function CharacterMap::gpTrigger1(%this, %val) {
   if (!%this.gpTrigger1Latch && %val >= %this.gpTrigger1High) {
      %this.gpTrigger1Latch = true;
      $mvTriggerCount4++;
   }

   if (%this.gpTrigger1Latch && %val < %this.gpTrigger1High) {
      %this.gpTrigger1Latch = false;
      $mvTriggerCount4++;
   }
}

CharacterMap.bindObj(gamepad, btn_b, gpTrigger1, CharacterMap);

// Actions
CharacterMap.bindCmd(keyboard, e, "commandToServer('use');", "");
CharacterMap.bindCmd(gamepad, btn_x, "commandToServer('use');", "");

CharacterMap.bindCmd(keyboard, tab, "commandToServer('swapWeapon');", "");
CharacterMap.bindCmd(gamepad, btn_y, "commandToServer('swapWeapon');", "");

CharacterMap.bindCmd(keyboard, q, "commandToServer('throwWeapon');", "");
CharacterMap.bindCmd(gamepad, btn_l, "commandToServer('throwWeapon');", "");
