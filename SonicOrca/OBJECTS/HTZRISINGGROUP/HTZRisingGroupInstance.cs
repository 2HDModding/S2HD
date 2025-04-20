// Decompiled with JetBrains decompiler
// Type: SONICORCA.OBJECTS.HTZRISINGGROUP.HTZRisingGroupInstance
// Assembly: S2HD, Version=2.0.1012.10521, Culture=neutral, PublicKeyToken=null
// MVID: 18631A0F-16CF-4E18-8563-1EC5E54750D6
// Assembly location: C:\Games\S2HD_2.0.1012-rc2\S2HD.exe

using SonicOrca.Core;
using SonicOrca.Geometry;
using System.Collections.Generic;

namespace SONICORCA.OBJECTS.HTZRISINGGROUP
{

    public class HTZRisingGroupInstance : ActiveObject
    {
      private Vector2i _size = new Vector2i(2048 /*0x0800*/, 512 /*0x0200*/);
      private List<ActiveObject> _groupMembers = new List<ActiveObject>();
      private Level _level;
      private int _state;
      private int _earthquakeY;
      private int _earthquakeTimer;
      private bool _earthquakeDirection;
      private int _earthquakeUpperBound;
      private int _earthquakeLowerBound;

      public ActiveObject SlotA { get; set; }

      public ActiveObject SlotB { get; set; }

      public ActiveObject SlotC { get; set; }

      public Vector2i Size
      {
        get => this._size;
        set
        {
          this._size = value;
          this.DesignBounds = new Rectanglei(-(this._size.X / 2), -(this._size.Y / 2), this._size.X, this._size.Y);
        }
      }

      public HTZRisingGroupInstance()
      {
        this.DesignBounds = new Rectanglei(-(this._size.X / 2), -(this._size.Y / 2), this._size.X, this._size.Y);
      }

      protected override void OnStart()
      {
        this._level = this.Level;
        switch (this._level.CurrentAct)
        {
          case 1:
            this.Act1Events();
            break;
          case 2:
            this.Act2Events();
            break;
        }
      }

      protected override void OnStop()
      {
        if (this.SlotA != null)
          this.SlotA.LockLifetime = false;
        if (this.SlotB != null)
          this.SlotB.LockLifetime = false;
        if (this.SlotC == null)
          return;
        this.SlotC.LockLifetime = false;
      }

      protected override void OnUpdate()
      {
        if (this.SlotA != null && !this._groupMembers.Contains(this.SlotA))
        {
          this.SlotA.LockLifetime = true;
          this._groupMembers.Add(this.SlotA);
          this.SlotA.Position = new Vector2i(this.SlotA.Position.X, this.SlotA.Position.Y + this._earthquakeUpperBound - this._earthquakeY);
        }
        if (this.SlotB != null && !this._groupMembers.Contains(this.SlotB))
        {
          this.SlotB.LockLifetime = true;
          this._groupMembers.Add(this.SlotB);
          this.SlotB.Position = new Vector2i(this.SlotB.Position.X, this.SlotB.Position.Y + this._earthquakeUpperBound - this._earthquakeY);
        }
        if (this.SlotC != null && !this._groupMembers.Contains(this.SlotC))
        {
          this.SlotC.LockLifetime = true;
          this._groupMembers.Add(this.SlotC);
          this.SlotC.Position = new Vector2i(this.SlotC.Position.X, this.SlotC.Position.Y + this._earthquakeUpperBound - this._earthquakeY);
        }
        switch (this._level.CurrentAct)
        {
          case 1:
            this.Act1Events();
            break;
          case 2:
            this.Act2Events();
            break;
        }
      }

      private void Act1Events()
      {
        Rectanglei bounds = (Rectanglei) this._level.Camera.Bounds;
        LevelMarker marker1 = this._level.GetMarker("eq_1_left_reset");
        LevelMarker marker2 = this._level.GetMarker("eq_1_left_begin");
        LevelMarker marker3 = this._level.GetMarker("eq_1_right_begin");
        LevelMarker marker4 = this._level.GetMarker("eq_1_right_reset");
        switch (this._state)
        {
          case 0:
            this._level.EarthquakeActive = false;
            if (bounds.X >= marker1.Position.X)
            {
              this._earthquakeUpperBound = 1280 /*0x0500*/;
              this._earthquakeLowerBound = 896;
              this._earthquakeY = this._earthquakeUpperBound;
              this._earthquakeTimer = 0;
              this._state = 1;
              break;
            }
            break;
          case 1:
            int x1 = bounds.X;
            Vector2i position = marker2.Position;
            int x2 = position.X;
            if (x1 >= x2)
            {
              int x3 = bounds.X;
              position = marker3.Position;
              int x4 = position.X;
              if (x3 < x4)
              {
                if (this._earthquakeDirection)
                {
                  if (this._earthquakeY == this._earthquakeLowerBound)
                  {
                    --this._earthquakeTimer;
                    if (this._earthquakeTimer < 0)
                    {
                      this._earthquakeTimer = 120;
                      this._earthquakeDirection = !this._earthquakeDirection;
                    }
                    this._level.EarthquakeActive = false;
                  }
                  else if (this._level.Ticks % 4 == 0)
                  {
                    this._earthquakeY -= 4;
                    this._level.EarthquakeActive = true;
                  }
                }
                else if (this._earthquakeY == this._earthquakeUpperBound)
                {
                  --this._earthquakeTimer;
                  if (this._earthquakeTimer < 0)
                  {
                    this._earthquakeTimer = 120;
                    this._earthquakeDirection = !this._earthquakeDirection;
                  }
                  this._level.EarthquakeActive = false;
                }
                else if (this._level.Ticks % 4 == 0)
                {
                  this._earthquakeY += 4;
                  this._level.EarthquakeActive = true;
                }
              }
            }
            int x5 = bounds.X;
            position = marker1.Position;
            int x6 = position.X;
            if (x5 < x6)
            {
              this._earthquakeDirection = false;
              this._state = 0;
              break;
            }
            int x7 = bounds.X;
            position = marker4.Position;
            int x8 = position.X;
            if (x7 >= x8)
            {
              this._earthquakeDirection = false;
              this._state = 2;
              break;
            }
            break;
          case 2:
            this._level.EarthquakeActive = false;
            if (bounds.X < marker4.Position.X)
            {
              this._earthquakeY = this._earthquakeUpperBound;
              this._earthquakeTimer = 0;
              this._state = 1;
              break;
            }
            break;
        }
        this.UpdateGroupPositions();
      }

      private void Act2Events()
      {
        Rectanglei bounds = (Rectanglei) this._level.Camera.Bounds;
        LevelMarker marker1 = this._level.GetMarker("eq_2_split");
        LevelMarker marker2 = this._level.GetMarker("eq_2a_left_reset");
        LevelMarker marker3 = this._level.GetMarker("eq_2a_left_begin");
        LevelMarker marker4 = this._level.GetMarker("eq_2a_right_begin");
        LevelMarker marker5 = this._level.GetMarker("eq_2a_right_reset");
        switch (this._state)
        {
          case 0:
            this._level.EarthquakeActive = false;
            if (bounds.X >= marker2.Position.X)
            {
              if (bounds.Y < marker1.Position.Y)
              {
                this._earthquakeUpperBound = 3328 /*0x0D00*/;
                this._earthquakeLowerBound = 0;
                this._earthquakeY = this._earthquakeUpperBound;
                this._state = 1;
                break;
              }
              this._earthquakeUpperBound = 3072 /*0x0C00*/;
              this._earthquakeLowerBound = 0;
              this._earthquakeY = this._earthquakeUpperBound;
              this._state = 4;
              break;
            }
            break;
          case 1:
            int x1 = bounds.X;
            Vector2i position1 = marker3.Position;
            int x2 = position1.X;
            if (x1 >= x2)
            {
              int x3 = bounds.X;
              position1 = marker4.Position;
              int x4 = position1.X;
              if (x3 < x4)
              {
                if (this._earthquakeDirection)
                {
                  if (this._earthquakeY == this._earthquakeLowerBound)
                  {
                    --this._earthquakeTimer;
                    if (this._earthquakeTimer < 0)
                    {
                      this._earthquakeTimer = 120;
                      this._earthquakeDirection = !this._earthquakeDirection;
                    }
                    this._level.EarthquakeActive = false;
                  }
                  else if (this._level.Ticks % 4 == 0)
                  {
                    this._earthquakeY -= 4;
                    this._level.EarthquakeActive = true;
                  }
                }
                else if (this._earthquakeY == this._earthquakeUpperBound)
                {
                  --this._earthquakeTimer;
                  if (this._earthquakeTimer < 0)
                  {
                    this._earthquakeTimer = 120;
                    this._earthquakeDirection = !this._earthquakeDirection;
                  }
                  this._level.EarthquakeActive = false;
                }
                else if (this._level.Ticks % 4 == 0)
                {
                  this._earthquakeY += 4;
                  this._level.EarthquakeActive = true;
                }
              }
            }
            int x5 = bounds.X;
            position1 = marker2.Position;
            int x6 = position1.X;
            if (x5 < x6)
            {
              this._earthquakeDirection = false;
              this._state = 0;
              break;
            }
            int x7 = bounds.X;
            position1 = marker5.Position;
            int x8 = position1.X;
            if (x7 >= x8)
            {
              this._earthquakeDirection = false;
              this._state = 2;
              break;
            }
            break;
          case 2:
            this._level.EarthquakeActive = false;
            if (bounds.X < marker5.Position.X)
            {
              this._earthquakeY = this._earthquakeUpperBound;
              this._earthquakeTimer = 0;
              this._state = 1;
              break;
            }
            break;
          case 4:
            int x9 = bounds.X;
            Vector2i position2 = marker3.Position;
            int x10 = position2.X;
            if (x9 >= x10)
            {
              int x11 = bounds.X;
              position2 = marker4.Position;
              int x12 = position2.X;
              if (x11 < x12)
              {
                if (this._earthquakeDirection)
                {
                  if (this._earthquakeY == this._earthquakeLowerBound)
                  {
                    --this._earthquakeTimer;
                    if (this._earthquakeTimer < 0)
                    {
                      this._earthquakeTimer = 120;
                      this._earthquakeDirection = !this._earthquakeDirection;
                    }
                    this._level.EarthquakeActive = false;
                  }
                  else if (this._level.Ticks % 4 == 0)
                  {
                    this._earthquakeY -= 4;
                    this._level.EarthquakeActive = true;
                  }
                }
                else if (this._earthquakeY == this._earthquakeUpperBound)
                {
                  --this._earthquakeTimer;
                  if (this._earthquakeTimer < 0)
                  {
                    this._earthquakeTimer = 120;
                    this._earthquakeDirection = !this._earthquakeDirection;
                  }
                  this._level.EarthquakeActive = false;
                }
                else if (this._level.Ticks % 4 == 0)
                {
                  this._earthquakeY += 4;
                  this._level.EarthquakeActive = true;
                }
              }
            }
            int x13 = bounds.X;
            position2 = marker2.Position;
            int x14 = position2.X;
            if (x13 < x14)
            {
              this._earthquakeDirection = false;
              this._state = 0;
              break;
            }
            int x15 = bounds.X;
            position2 = marker5.Position;
            int x16 = position2.X;
            if (x15 >= x16)
            {
              this._earthquakeDirection = false;
              this._state = 5;
              break;
            }
            break;
          case 5:
            this._level.EarthquakeActive = false;
            if (bounds.X < marker5.Position.X)
            {
              this._earthquakeY = this._earthquakeUpperBound;
              this._earthquakeTimer = 0;
              this._state = 4;
              break;
            }
            break;
        }
        this.UpdateGroupPositions();
      }

      private void UpdateGroupPositions()
      {
        if (!this._level.EarthquakeActive)
          return;
        int num = this._earthquakeDirection ? -4 : 4;
        if (this._level.Ticks % 4 != 0)
          return;
        foreach (ActiveObject groupMember in this._groupMembers)
        {
          Vector2i position = groupMember.Position;
          position.Y += num;
          groupMember.Position = position;
        }
      }
    }
}
