const express = require('express');
const router = express.Router();

// Workspace model
const Workspace = require('../models/workspace');

// @route   GET /api/workspace/
// @desc    Get all workspaces
// @access  Public
router.get('/', async (req, res) => {
  try {
    const workspace = await Workspace.find({});
    res.send({ workspace })
  } catch(err) {
    res.status(400).send({ error: err });
  }
});

// @route   GET /api/workspace/:id
// @desc    Get a specific workspace
// @access  Public
router.get('/:id', async (req, res) => {
  try {
    const workspace = await Workspace.findById(req.params.id);
    res.send({ workspace });
  } catch (err) {
    res.status(404).send({ message: 'Workspace not found!' });
  }
});

// @route   POST /api/workspace/
// @desc    Create a workspace
// @access  Public
router.post('/', async (req, res) => {
  try {
    const newWorkspace = await Workspace.create({ name: req.body.name, email: req.body.email, enrollnumber: req.body.enrollnumber });
     res.send({ newWorkspace });
  } catch(err) {
    res.status(400).send({ error: err });
  }

});

// @route   PUT /api/Workspace/:id
// @desc    Update a workspace
// @access  Public
router.put('/:id', async (req, res) => {
  try {
    const updatedWorkspace = await Workspace.findByIdAndUpdate(req.params.id, req.body);
     res.send({ message: 'The workspace was updated' });
  } catch(err) {
    res.status(400).send({ error: err });
  }
});

// @route   DELETE /api/workspace/:id
// @desc    Delete a workspace
// @access  Public
router.delete('/:id', async (req, res) => {
  try {
    const removeWorkspace = await Workspace.findByIdAndRemove(req.params.id);
     res.send({ message: 'The workspace was removed' });
  } catch(err) {
    res.status(400).send({ error: err });
  }
});


module.exports = router;