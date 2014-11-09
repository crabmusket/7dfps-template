new ActionMap(InGameMap);

// Metaprogramming!
function setGlobal(%var) {
   %funcName = "_set_"@%var;
   eval("function" SPC %funcName @ "(%val) { $"@%var@" = %val;}");
   return %funcName;
}

//-----------------------------------------------------------------------------
// Keyboard controls
function InGameMap::yaw(%this, %amount) {
   $mvYaw += %amount * 0.01;
}

function InGameMap::pitch(%this, %amount) {
   $mvPitch += %amount * 0.01;
}

InGameMap.bindCmd(keyboard, escape, %escape, "");
InGameMap.bindCmd(gamepad, btn_back, %escape, "");

InGameMap.bind(keyboard, w, setGlobal(mvForwardAction));
InGameMap.bind(keyboard, s, setGlobal(mvBackwardAction));
InGameMap.bind(keyboard, a, setGlobal(mvLeftAction));
InGameMap.bind(keyboard, d, setGlobal(mvRightAction));
InGameMap.bindCmd(keyboard, space, "$mvTriggerCount2++;", "$mvTriggerCount2++;");
InGameMap.bindObj(mouse, "xaxis", yaw, InGameMap);
InGameMap.bindObj(mouse, "yaxis", pitch, InGameMap);

InGameMap.bindCmd(keyboard, tab, "ServerConnection.setFirstPerson(!ServerConnection.isFirstPerson());", "");

InGameMap.bind(keyboard, f11, toggleEditor);

//-----------------------------------------------------------------------------
// Gamepad controls
function InGameMap::gpYaw(%this, %amount) {
   if (%amount > 0) {
      $mvYawLeftSpeed = %amount;
      $mvYawRightSpeed = 0;
   } else {
      $mvYawLeftSpeed = %amount;
      $mvYawRightSpeed = 0;
   }
}

function InGameMap::gpPitch(%this, %amount) {
   if (%amount > 0) {
      $mvPitchDownSpeed = %amount;
      $mvPitchUpSpeed = 0;
   } else {
      $mvPitchDownSpeed = %amount;
      $mvPitchUpSpeed = 0;
   }
}

InGameMap.bind(gamepad, thumbrx, "D", "-0.23 0.23", gpYaw);
InGameMap.bind(gamepad, thumbry, "D", "-0.23 0.23", gpPitch);
InGameMap.bind(gamepad, thumblx, "D", "-0.23 0.23", setGlobal(mvRightAction));
InGameMap.bind(gamepad, thumbly, "D", "-0.23 0.23", setGlobal(mvForwardAction));
