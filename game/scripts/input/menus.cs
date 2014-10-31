new ActionMap(MenuActionMap);

InputEvents.registerEvent(EvtAdvance);
%advance = "InputEvents.postEvent(EvtAdvance);";
MenuActionMap.bindCmd(keyboard, space, %advance, "");
MenuActionMap.bindCmd(keyboard, enter, %advance, "");
//MenuActionMap.bindCmd(mouse, button0, %advance, "");
MenuActionMap.bindCmd(gamepad, btn_a, %advance, "");
MenuActionMap.bindCmd(gamepad, dpadr, %advance, "");

InputEvents.registerEvent(EvtEscape);
%escape = "InputEvents.postEvent(EvtEscape);";
MenuActionMap.bindCmd(keyboard, escape, %escape, "");
MenuActionMap.bindCmd(gamepad, btn_back, %escape, "");
