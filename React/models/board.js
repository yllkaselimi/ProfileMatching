const mongoose = require('mongoose');

const boardSchema = new mongoose.Schema({
    BoardID: {
    type: String,
    required: true,
  },
  WorkspaceID: {
    type: String,
  },
  BoardName: {
    type: String,
  },
});

module.exports = mongoose.model('Board', boardSchema);
