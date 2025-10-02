// SPDX-License-Identifier: MIT
pragma solidity ^0.8.0;

contract ScoreRecorder {
    struct Score {
        address player;
        uint256 score;
        uint256 timestamp;
        uint256 level;
        string fileHash;
    }

    Score[] public scores;

    event ScoreSaved(
        address indexed player,
        uint256 score,
        uint256 timestamp,
        uint256 level,
        string fileHash
    );

    function saveScore(
        uint256 _score,
        uint256 _level,
        string memory _fileHash
    ) public {
        scores.push(Score({
            player: msg.sender,
            score: _score,
            timestamp: block.timestamp,
            level: _level,
            fileHash: _fileHash
        }));

        emit ScoreSaved(msg.sender, _score, block.timestamp, _level, _fileHash);
    }

    function getScore(uint256 index) public view returns (
        address, uint256, uint256, uint256, string memory
    ) {
        Score memory s = scores[index];
        return (s.player, s.score, s.timestamp, s.level, s.fileHash);
    }

    function getScoreCount() public view returns (uint256) {
        return scores.length;
    }
}
