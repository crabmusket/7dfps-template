function TrainingLevel::onLoad(%this) {
   LevelEvents.subscribe(%this, EvtLevelLoaded);
}

function TrainingLevel::onDestroy(%this) {
   LevelEvents.removeAll(%this);
}

function TrainingLevel::onEvtLevelLoaded(%this) {
   %this.state = new ScriptMsgListener() {
      class = StateMachine;
      state = null;
      transition[null, load] = playing;
   };

   %this.state.onEvent(load);
}

function TrainingLevel::onDestroy(%this) {
}
