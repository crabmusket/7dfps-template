new EventManager(InputEvents) {
   queue = InputEventQueue;
};
// EvtAdvance and EvtEscape are typically used for menu navigation. Advance is
// for affirmitive responses while escape is usually 'no thanks' or 'get me out
// of here'.
InputEvents.registerEvent(EvtAdvance);
InputEvents.registerEvent(EvtEscape);
// The four directions are also used for menu navigation. Forwards and Back are
// horizontal, whereas Prev and Next are vertical (up and down respectively).
InputEvents.registerEvent(EvtForwards);
InputEvents.registerEvent(EvtBackwards);
InputEvents.registerEvent(EvtPrev);
InputEvents.registerEvent(EvtNext);

// When the game starts, we want to enable all inputs, including controller input.
// This lets us use all controls during the menu startup sequence.
GameEvents.subscribe(InputEvents, EvtStart);
function InputEvents::onEvtStart(%this) {
   $enableDirectInput = 1;
   activateDirectInput();
}

exec("./menus.cs");
exec("./ingame.cs");
