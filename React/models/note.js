const mongoose = require('mongoose');

const noteSchema = new mongoose.Schema({
    NoteID: {
    type: String,
    required: true,
  },
  BoardID: {
    type: String,
  },
  NoteText: {
    type: String,
  },
  UserID: {
    type: String,
  },
});

module.exports = mongoose.model('Note', noteSchema);
