new ActionMap(MenuActionMap);

%gamepad = "InputEvents.setInputMethod(gamepad);";
%keyboard = "InputEvents.setInputMethod(keyboard);";

%advance = "InputEvents.postEvent(EvtAdvance);";
MenuActionMap.bindCmd(keyboard, space, %advance, %keyboard);
MenuActionMap.bindCmd(keyboard, enter, %advance, %keyboard);
MenuActionMap.bindCmd(gamepad, btn_a, %advance, %gamepad);

%escape = "InputEvents.postEvent(EvtEscape);";
MenuActionMap.bindCmd(keyboard, escape, %escape, "");
MenuActionMap.bindCmd(gamepad, btn_back, %escape, %gamepad);
MenuActionMap.bindCmd(gamepad, btn_b, %escape, %gamepad);

%forw = "InputEvents.postEvent(EvtForwards);";
MenuActionMap.bindCmd(keyboard, right, %forw, %keyboard);
MenuActionMap.bindCmd(gamepad, dpadr, %forw, %gamepad);

%back = "InputEvents.postEvent(EvtBackwards);";
MenuActionMap.bindCmd(keyboard, left, %back, %keyboard);
MenuActionMap.bindCmd(gamepad, dpadl, %back, %gamepad);

%prev = "InputEvents.postEvent(EvtPrev);";
MenuActionMap.bindCmd(keyboard, up, %prev, %keyboard);
MenuActionMap.bindCmd(gamepad, dpadu, %prev, %gamepad);

%next = "InputEvents.postEvent(EvtNext);";
MenuActionMap.bindCmd(keyboard, down, %next, %keyboard);
MenuActionMap.bindCmd(gamepad, dpadd, %next, %gamepad);

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
      InputEvents.setInputMethod(gamepad);
   }

   if (%val < -%this.latchUpper && %this.latchLX == false) {
      %this.latchLX = true;
      InputEvents.postEvent(EvtBackwards);
      InputEvents.setInputMethod(gamepad);
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
      InputEvents.setInputMethod(gamepad);
   }

   if (%val < -%this.latchUpper && %this.latchLY == false) {
      %this.latchLY = true;
      InputEvents.postEvent(EvtNext);
      InputEvents.setInputMethod(gamepad);
   }

   if (%val > -%this.latchLower && %val < %this.latchLower && %this.latchLY == true) {
      %this.latchLY = false;
   }
}
