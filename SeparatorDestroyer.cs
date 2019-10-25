// 
//  MIT License
// 
//  Copyright (c) 2017-2019 William "Xyphos" Scott (TheGreatXyphos@gmail.com)
// 
//  Permission is hereby granted, free of charge, to any person obtaining a copy
//  of this software and associated documentation files (the "Software"), to deal
//  in the Software without restriction, including without limitation the rights
//  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//  copies of the Software, and to permit persons to whom the Software is
//  furnished to do so, subject to the following conditions:
// 
//  The above copyright notice and this permission notice shall be included in all
//  copies or substantial portions of the Software.
// 
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//  SOFTWARE.

using System;
using UnityEngine;

namespace XyphosAerospace
{
  // ReSharper disable UnusedMember.Global
  public class SeparatorDestroyer : PartModule
  {
    private ModuleDecouple _moduleDecouple;

    [KSPField(
        advancedTweakable = true,
        guiActive         = true,
        guiActiveEditor   = false,
        isPersistant      = true,
        guiName           = "Destructible"
      )]
    public bool Destructible;


    [KSPEvent(
        advancedTweakable = true,
        guiActive         = false,
        guiActiveEditor   = true,
        guiName           = "Destructible: No"
      )]
    public void ToggleDestructible()
    {
      Destructible = !Destructible;
      UpdateDestructible();
    }

    /// <summary>
    ///   Updates the destructible.
    /// </summary>
    private void UpdateDestructible() => Events[actionName: "ToggleDestructible"].guiName = "Destructible: " + (Destructible ? "Yes" : "No");

    /// <summary>
    ///   Called when [start].
    /// </summary>
    /// <param name="state">The state.</param>
    public override void OnStart(StartState state) => UpdateDestructible();

    /// <summary>
    ///   Called when [awake].
    /// </summary>
    public override void OnAwake()
    {
      try
      {
        base.OnAwake();
        _moduleDecouple = part.Modules.GetModule<ModuleDecouple>();

        if (_moduleDecouple == null
         || _moduleDecouple.isOmniDecoupler)
          return;

        // normal decouplers shouldn't be considered and therefore nullified, with their GUI fields disabled.
        _moduleDecouple                                          = null;
        Destructible                                             = false;
        Events[actionName: "ToggleDestructible"].guiActiveEditor = false;
        Fields[fieldName: "Destructible"].guiActive              = false;
      }
      catch (Exception e) { }
    }

    /// <summary>
    ///   Called when [fixed update].
    /// </summary>
    public void FixedUpdate()
    {
      try
      {
        if (!HighLogic.LoadedSceneIsFlight
         || !Destructible
         || _moduleDecouple == null
         || !_moduleDecouple.isDecoupled)
          return;

        //*
        ScreenMessages.PostScreenMessage(
            message: "Stack Separator Destroyed",
            duration: 5f,
            style: ScreenMessageStyle.UPPER_CENTER,
            color: Color.red
          );
        //*/

        part.explode();
      }
      catch (Exception e) { }
    }

    /// <summary>
    ///   Gets the information.
    /// </summary>
    /// <returns></returns>
    public override string GetInfo() => _moduleDecouple != null && _moduleDecouple.isOmniDecoupler ? "Can be destroyed when decoupled" : string.Empty;
  }
}
