const ScoreRecorder = artifacts.require("ScoreRecorder");

module.exports = function (deployer) {
  deployer.deploy(ScoreRecorder);
};