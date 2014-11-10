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
// This event is an interesting one. Based on which functions are being called,
// we can actually detect changes in the player's current input method. This is
// handled in the utility function InputEvents::setInputMethod below.
InputEvents.registerEvent(EvtChangeInputMethod);

// When the game starts, we want to enable all inputs, including controller input.
// This lets us use all controls during the menu startup sequence.
GameEvents.subscribe(InputEvents, EvtStart);
function InputEvents::onEvtStart(%this, %args) {
   $enableDirectInput = 1;
   activateDirectInput();
}

// Sends an EvtChangeInputMethod if the method has changed. You probably shouldn't
// call this all the time, especially if lots of objects start subscribing to
// that event. Catch the most important inputs to denote method change.
function InputEvents::setInputMethod(%this, %method) {
   if (%method !$= %this.inputMethod) {
      %this.inputMethod = %method;
      %this.postEvent(EvtChangeInputMethod, %method);
   }
}

exec("./menus.cs");
exec("./ingame.cs");
