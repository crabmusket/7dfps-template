datablock PlayerData(EnemySoldier) {
   shapeFile = "art/characters/soldier.dae";
};

function EnemySoldier::onAdd(%this, %obj) {
   %obj.setSkinName(enemy);
}
