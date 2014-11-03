new ActionMap(MenuActionMap);

%advance = "InputEvents.postEvent(EvtAdvance);";
MenuActionMap.bindCmd(keyboard, space, %advance, "");
MenuActionMap.bindCmd(keyboard, enter, %advance, "");
MenuActionMap.bindCmd(gamepad, btn_a, %advance, "");

%escape = "InputEvents.postEvent(EvtEscape);";
MenuActionMap.bindCmd(keyboard, escape, %escape, "");
MenuActionMap.bindCmd(gamepad, btn_back, %escape, "");
MenuActionMap.bindCmd(gamepad, btn_b, %escape, "");

%forw = "InputEvents.postEvent(EvtForwards);";
MenuActionMap.bindCmd(keyboard, right, %forw, "");
MenuActionMap.bindCmd(gamepad, dpadr, %forw, "");

%back = "InputEvents.postEvent(EvtBackwards);";
MenuActionMap.bindCmd(keyboard, left, %back, "");
MenuActionMap.bindCmd(gamepad, dpadl, %back, "");

%prev = "InputEvents.postEvent(EvtPrev);";
MenuActionMap.bindCmd(keyboard, up, %prev, "");
MenuActionMap.bindCmd(gamepad, dpadu, %prev, "");

%next = "InputEvents.postEvent(EvtNext);";
MenuActionMap.bindCmd(keyboard, down, %next, "");
MenuActionMap.bindCmd(gamepad, dpadd, %next, "");

// Special gamepad controls for using the left thumbstick to navigate menus.
MenuActionMap.latchUpper = 0.85;
MenuActionMap.latchLower = 0.5;
MenuActionMap.bindObj(gamepad, thumblx, "D", "-0.23 0.23", directionLatchLX, MenuActionMap);
MenuActionMap.bindObj(gamepad, thumbly, "D", "-0.23 0.23", directionLatchLY, MenuActionMap);

MenuActionMap.latchLX = false;
function MenuActionMap::directionLatchLX(%this, %val) {
   if (%val > %this.latchUpper && %this.latchLX == false) {
      %this.latchLX = true;
      InputEvents.postEvent(EvtForwards);
   }

   if (%val < -%this.latchUpper && %this.latchLX == false) {
      %this.latchLX = true;
      InputEvents.postEvent(EvtBack);
   }

   if (%val > -%this.latchLower && %val < %this.latchLower && %this.latchLX == true) {
      %this.latchLX = false;
   }
}

MenuActionMap.latchLY = false;
function MenuActionMap::directionLatchLY(%this, %val) {
   if (%val > %this.latchUpper && %this.latchLY == false) {
      %this.latchLY = true;
      InputEvents.postEvent(EvtPrev);
   }

   if (%val < -%this.latchUpper && %this.latchLY == false) {
      %this.latchLY = true;
      InputEvents.postEvent(EvtNext);
   }

   if (%val > -%this.latchLower && %val < %this.latchLower && %this.latchLY == true) {
      %this.latchLY = false;
   }
}
