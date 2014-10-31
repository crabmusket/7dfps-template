new EventManager(InputEvents) {
   queue = InputEventQueue;
};

GameEvents.subscribe(InputEvents, EvtStart);

function InputEvents::onEvtStart(%this) {
   $enableDirectInput = 1;
   activateDirectInput();
}

exec("./menus.cs");
