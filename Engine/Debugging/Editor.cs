using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text.RegularExpressions;
using Engine.Screens;
using ImGuiNET;
using Microsoft.Xna.Framework;
using Vector4 = System.Numerics.Vector4;

namespace Engine.Debugging;

public static class Editor
{
    internal const int OFFSET_FROM_START_X = 150;
    internal const int ITEM_WIDTH = 75;

    public static bool ShowDebug { get => showDebug; private set => showDebug = value; }
    private static bool showDebug = false;

    public static bool ShowFps { get => showFps; private set => showFps = value; }
    private static bool showFps = false;
    
    private static GameObject selectedObject;
    
    public static void DrawDebugEditor(this GameObjectScreen screen)
    {
        ImGui.SetNextWindowPos(System.Numerics.Vector2.Zero, ImGuiCond.Always);
        if (ImGui.Begin(FormatName(screen.Name), ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.MenuBar))
        {
            if (ImGui.BeginMenuBar())
            {
                if (ImGui.BeginMenu("File"))
                {
                    ImGui.MenuItem("Save");
                    if (ImGui.MenuItem("Exit")){} /*Runtime.Exit();*/
                    ImGui.EndMenu();
                }

                if (ImGui.BeginMenu("Debug"))
                {
                    ImGui.MenuItem("Show FPS", "", ref showFps);
                    ImGui.MenuItem("Show Colliders", "", ref showDebug);

                    ImGui.EndMenu();
                }

                ImGui.EndMenuBar();
            }

            float width = ImGui.GetContentRegionAvail().X;
            float height = ImGui.GetContentRegionAvail().Y;
                
            ImGui.BeginChild("Hierarchy", new System.Numerics.Vector2(width * 0.3f, height * 0.65f), ImGuiChildFlags.Border);
            ImGui.Text("Hierarchy");
            ImGui.Separator();
                
            foreach (var obj in screen.GameObjects)
            {
                if (ImGui.Selectable(obj.Name, selectedObject == obj))
                {
                    selectedObject = obj;
                }
            }

            ImGui.EndChild();
            ImGui.SameLine();
                
            ImGui.BeginChild("Inspector", new System.Numerics.Vector2(0, height * 0.65f), ImGuiChildFlags.Border);
            ImGui.Text("Inspector");
            ImGui.Separator();
                
            if (selectedObject != null)
            {
                ImGui.BeginChild("Transform", new System.Numerics.Vector2(0, 0), ImGuiChildFlags.Border | ImGuiChildFlags.AutoResizeY);
                ImGui.Text("Transform");
                ImGui.Spacing();
                
                Vector2 position = selectedObject.Transform.Position;
                if (DrawVector2("Position", ref position))
                    selectedObject.Transform.Position = position;
                
                float rotation = selectedObject.Transform.Rotation;
                if(DrawFloat("Rotation", ref rotation))
                    selectedObject.Transform.Rotation = rotation;
                
                Vector2 scale = selectedObject.Transform.Scale;
                if (DrawVector2("Scale", ref scale))
                    selectedObject.Transform.Scale = scale;
                
                ImGui.EndChild();
                
                foreach (var script in selectedObject.scripts)
                {
                    ImGui.BeginChild(script.ToString(), new System.Numerics.Vector2(0, 0), ImGuiChildFlags.Border | ImGuiChildFlags.AutoResizeY);
                    ImGui.Text(FormatName(script.GetType().Name));
                    
                    bool enabled = script.Enabled;
                    float checkboxWidth = ImGui.GetFrameHeight();
                    float rightX = ImGui.GetWindowSize().X - ImGui.GetStyle().WindowPadding.X - checkboxWidth;
                    ImGui.SameLine(rightX);

                    if (ImGui.Checkbox("##Enabled", ref enabled))
                        script.Enabled = enabled;
                    
                    ImGui.Spacing();

                    foreach (var member in script.GetType().GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
                    {
                        if (member is not FieldInfo && member is not PropertyInfo)
                            continue;

                        if (member.GetCustomAttribute<DontInspectAttribute>() != null)
                            continue;
                        
                        if (member is PropertyInfo enabledProperty && enabledProperty.DeclaringType == typeof(Script) && enabledProperty.Name == nameof(Script.Enabled))
                            continue;
                        
                        bool isPublic = member switch
                        {
                            FieldInfo field => field.IsPublic,

                            PropertyInfo property =>
                                property.GetMethod?.IsPublic == true ||
                                property.SetMethod?.IsPublic == true,

                            _ => false
                        };

                        bool hasInspect = member.GetCustomAttribute<InspectAttribute>() != null;
                        bool hasReadOnly = member.GetCustomAttribute<ReadOnlyAttribute>() != null;

                        
                        if (!isPublic && !hasInspect && !hasReadOnly)
                            continue;

                        DrawMember(script, member);
                    }
                    
                    ImGui.EndChild();
                }
            }

                
            ImGui.EndChild();
            
            ImGui.BeginChild("Console", new System.Numerics.Vector2(0, height * 0.35f), ImGuiChildFlags.AutoResizeY | ImGuiChildFlags.Border);
            ImGui.Text("Console");
            ImGui.Separator();
            
            ImGui.BeginChild("Console Messages", new System.Numerics.Vector2(0, 0), ImGuiChildFlags.AlwaysAutoResize);

            foreach (var log in Debug.ConsoleLogs)
            {
                ImGui.PushStyleColor(ImGuiCol.Text, ConsoleWriter.GetImGuiColor(log.Color));
                ImGui.TextUnformatted(log.Text);
                ImGui.PopStyleColor();
            }
            
            if (ImGui.GetScrollY() >= ImGui.GetScrollMaxY())
                ImGui.SetScrollHereY(1.0f);
            ImGui.EndChild();
            

            ImGui.EndChild();
            
            ImGui.End();
        }

        if (showFps)
        {
            ImGui.SetNextWindowPos(new System.Numerics.Vector2(ImGui.GetIO().DisplaySize.X, 0), ImGuiCond.Always, new System.Numerics.Vector2(1, 0));
            ImGui.SetNextWindowBgAlpha(0.8f);

            if (ImGui.Begin("FPS", ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.AlwaysAutoResize | ImGuiWindowFlags.NoSavedSettings))
                ImGui.Text($"FPS: {ImGui.GetIO().Framerate:F0}");

            ImGui.End();
        }
    }
    
    public static bool DrawVector2(string label, ref Vector2 value, bool readOnly = false)
    {
        bool changed = false;

        ImGui.Text(FormatName(label));
        
        float labelWidth = ImGui.CalcTextSize(label).X;
        float offset = MathF.Max(OFFSET_FROM_START_X, ImGui.GetCursorPosX() + labelWidth + ImGui.GetStyle().ItemSpacing.X);
        ImGui.SameLine(offset);

        float x = value.X;
        float y = value.Y;
        
        if (readOnly)
            ImGui.BeginDisabled();

        ImGui.SetNextItemWidth(ITEM_WIDTH);
        if (ImGui.DragFloat("X##" + label, ref x, 1.0f, 0.0f, 0.0f, "%.2f"))
            changed = true;

        ImGui.SameLine();

        ImGui.SetNextItemWidth(ITEM_WIDTH);
        if (ImGui.DragFloat("Y##" + label, ref y, 1.0f, 0.0f, 0.0f, "%.2f"))
            changed = true;

        value.X = x;
        value.Y = y;

        if (readOnly)
            ImGui.EndDisabled();
        
        return changed;
    }
    
    public static bool DrawFloat(string label, ref float value, bool readOnly = false)
    {
        bool changed = false;
        
        ImGui.Text(FormatName(label));
        
        float labelWidth = ImGui.CalcTextSize(label).X;
        float offset = MathF.Max(OFFSET_FROM_START_X, ImGui.GetCursorPosX() + labelWidth + ImGui.GetStyle().ItemSpacing.X);
        ImGui.SameLine(offset);
        
        if (readOnly)
            ImGui.BeginDisabled();
        
        ImGui.SetNextItemWidth(ITEM_WIDTH);
        if (ImGui.DragFloat("##" + label, ref value, 1.0f, 0.0f, 0.0f, "%.2f"))
            changed = true;

        if (readOnly)
            ImGui.EndDisabled();
        
        return changed;
    }
    
    public static bool DrawInt(string label, ref int value, bool readOnly = false)
    {
        bool changed = false;
        
        ImGui.Text(FormatName(label));
        
        float labelWidth = ImGui.CalcTextSize(label).X;
        float offset = MathF.Max(OFFSET_FROM_START_X, ImGui.GetCursorPosX() + labelWidth + ImGui.GetStyle().ItemSpacing.X);
        ImGui.SameLine(offset);
        
        if (readOnly)
            ImGui.BeginDisabled();
        
        ImGui.SetNextItemWidth(ITEM_WIDTH);
        if (ImGui.DragInt("##" + label, ref value))
            changed = true;

        if (readOnly)
            ImGui.EndDisabled();
        
        return changed;
    }
    
    public static bool DrawCheckbox(string label, ref bool value, bool readOnly = false)
    {
        bool changed = false;
        
        ImGui.Text(FormatName(label));
        
        float labelWidth = ImGui.CalcTextSize(label).X;
        float offset = MathF.Max(OFFSET_FROM_START_X, ImGui.GetCursorPosX() + labelWidth + ImGui.GetStyle().ItemSpacing.X);
        ImGui.SameLine(offset);
        
        if (readOnly)
            ImGui.BeginDisabled();
        
        ImGui.SetNextItemWidth(ITEM_WIDTH);
        if (ImGui.Checkbox("##" + label, ref value))
            changed = true;

        if (readOnly)
            ImGui.EndDisabled();
        
        return changed;
    }
    
    public static bool DrawString(string label, ref string value, bool readOnly = false)
    {
        bool changed = false;
        
        if (readOnly)
            ImGui.BeginDisabled();

        ImGui.Text(FormatName(label));
        
        float labelWidth = ImGui.CalcTextSize(label).X;
        float offset = MathF.Max(OFFSET_FROM_START_X, ImGui.GetCursorPosX() + labelWidth + ImGui.GetStyle().ItemSpacing.X);
        ImGui.SameLine(offset);
        
        ImGui.SetNextItemWidth(ITEM_WIDTH);
        if (ImGui.InputText("##" + label, ref value, 256))
            changed = true;

        if (readOnly)
            ImGui.EndDisabled();
        
        return changed;
    }
    
    public static bool DrawColor(string label, ref Color value, bool readOnly = false)
    {
        bool changed = false;
        
        if (readOnly)
            ImGui.BeginDisabled();

        ImGui.Text(FormatName(label));
        
        float labelWidth = ImGui.CalcTextSize(label).X;
        float offset = MathF.Max(OFFSET_FROM_START_X, ImGui.GetCursorPosX() + labelWidth + ImGui.GetStyle().ItemSpacing.X);
        ImGui.SameLine(offset);
        
        ImGui.SetNextItemWidth(ITEM_WIDTH * 2.0f);
        Vector4 col = new Vector4(value.R / 255.0f, value.G / 255.0f, value.B / 255.0f, value.A / 255.0f);
        if (ImGui.ColorEdit4("##" + label, ref col))
        {
            changed = true;
            value = new Color(col.X, col.Y, col.Z, col.W);
        }

        if (readOnly)
            ImGui.EndDisabled();
        
        return changed;
    }

    public static bool DrawEnum(string label, Type enumType, ref object value, bool readOnly = false)
    {
        bool changed = false;
        
        if (readOnly)
            ImGui.BeginDisabled();

        ImGui.Text(FormatName(label));
        
        float labelWidth = ImGui.CalcTextSize(label).X;
        float offset = MathF.Max(OFFSET_FROM_START_X, ImGui.GetCursorPosX() + labelWidth + ImGui.GetStyle().ItemSpacing.X);
        ImGui.SameLine(offset);
        
        ImGui.SetNextItemWidth(ITEM_WIDTH * 2);
        
        Array values = Enum.GetValues(enumType);

        if (ImGui.BeginCombo("##" + label, value.ToString()))
        {
            foreach (object enumValue in values)
            {
                bool selected = Equals(value, enumValue);

                if (ImGui.Selectable(enumValue.ToString(), selected))
                {
                    value = enumValue;
                    changed = true;
                }

                if (selected)
                    ImGui.SetItemDefaultFocus();
            }

            ImGui.EndCombo();
        }
        
        if (readOnly)
            ImGui.EndDisabled();
        
        return changed;
    }

    [RequiresDynamicCode("Calls System.Enum.GetValues(Type)")]
    private static void DrawMember(object script, MemberInfo member)
    {
        object? value = GetMemberValue(script, member);
        
        bool readOnly =
            member is PropertyInfo property && property.SetMethod == null ||
            member.GetCustomAttribute<ReadOnlyAttribute>() != null;
        
        switch (GetMemberType(member))
        {
            case { } t when t == typeof(int):
                int intValue = (int)(value ?? 0);
                
                if (DrawInt(member.Name, ref intValue, readOnly) && !readOnly)
                    SetMemberValue(script, member, intValue);
                break;

            case { } t when t == typeof(float):
                float floatValue = (float)(value ?? 0.0f);
                
                if (DrawFloat(member.Name, ref floatValue, readOnly) && !readOnly)
                    SetMemberValue(script, member, floatValue);     
                break;

            case { } t when t == typeof(bool):
                bool boolValue = (bool)(value ?? false);
                
                if (DrawCheckbox(member.Name, ref boolValue, readOnly) && !readOnly)
                    SetMemberValue(script, member, boolValue);
                break;

            case { } t when t == typeof(string):
                string stringValue = (string)(value ?? "");
                
                if (DrawString(member.Name, ref stringValue, readOnly) && !readOnly)
                    SetMemberValue(script, member, stringValue);
                break;
            
            case { } t when t == typeof(Color):
                Color colorValue = (Color)(value ?? Color.White);
                
                if (DrawColor(member.Name, ref colorValue, readOnly) && !readOnly)
                    SetMemberValue(script, member, colorValue);
                break;
            
            case { } t when t.IsEnum:
                object enumValue = value ?? Enum.GetValues(t).GetValue(0)!;
                
                if (DrawEnum(member.Name, t, ref enumValue, readOnly) && !readOnly)
                    SetMemberValue(script, member, enumValue);
                break;


        }
    }
    
    private static Type GetMemberType(MemberInfo member)
    {
        return member switch
        {
            FieldInfo field => field.FieldType,
            PropertyInfo property => property.PropertyType,
            _ => throw new ArgumentException("Unsupported member type")
        };
    }
    
    private static object? GetMemberValue(object instance, MemberInfo member)
    {
        return member switch
        {
            FieldInfo field => field.GetValue(instance),
            PropertyInfo property => property.GetValue(instance),
            _ => null
        };
    }
    
    private static void SetMemberValue(object script, MemberInfo member, object value)
    {
        switch (member)
        {
            case FieldInfo field:
                field.SetValue(script, value);
                break;

            case PropertyInfo property:
                if (property.SetMethod != null)
                    property.SetValue(script, value);
                break;
        }
    }
    
    public static string FormatName(string name)
    {
        string spaced = Regex.Replace(
            name,
            @"(?<=[a-z])(?=[A-Z])|(?<=[A-Z])(?=[A-Z][a-z])|(?<=[a-zA-Z])(?=[0-9])",
            " "
        ).Trim();
        
        if (string.IsNullOrEmpty(spaced))
            return spaced;

        return char.ToUpper(spaced[0]) + spaced[1..];
    }}