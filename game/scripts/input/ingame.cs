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

InGameMap.bindCmd(keyboard, tab, "ServerConnection.setFirstPerson(!ServerConnection.isFirstPerson());", "");

//-----------------------------------------------------------------------------
// Controlling a character
new ActionMap(CharacterMap);

// Keyboard controls
function CharacterMap::yaw(%this, %amount) {
   $mvYaw += %amount * 0.01;
}

function CharacterMap::pitch(%this, %amount) {
   $mvPitch += %amount * 0.01;
}

CharacterMap.bind(keyboard, w, setGlobal(mvForwardAction));
CharacterMap.bind(keyboard, s, setGlobal(mvBackwardAction));
CharacterMap.bind(keyboard, a, setGlobal(mvLeftAction));
CharacterMap.bind(keyboard, d, setGlobal(mvRightAction));
CharacterMap.bindCmd(keyboard, space, "$mvTriggerCount2++;", "$mvTriggerCount2++;");
CharacterMap.bindObj(mouse, "xaxis", yaw, CharacterMap);
CharacterMap.bindObj(mouse, "yaxis", pitch, CharacterMap);

// Gamepad controls
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
CharacterMap.bind(gamepad, thumblx, "D", "-0.23 0.23", setGlobal(mvRightAction));
CharacterMap.bind(gamepad, thumbly, "D", "-0.23 0.23", setGlobal(mvForwardAction));
