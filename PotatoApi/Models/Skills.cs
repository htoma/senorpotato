using System.Collections.Generic;

namespace PotatoApi.Models
{
    public class Skills
    {
        public Skills()
        {
            _skillSet = new Dictionary<ESkill, int>
            {
                {ESkill.Defence, 0},
                {ESkill.Attack, 0},
                {ESkill.Penalty, 0},
                {ESkill.Stamina, 0}
            };
        }

        public int Defence
        {
            get { return _skillSet[ESkill.Defence]; }
            set { _skillSet[ESkill.Defence] = value; }
        }

        public int Attack
        {
            get { return _skillSet[ESkill.Attack]; }
            set { _skillSet[ESkill.Attack] = value; }
        }

        public int Penalty
        {
            get { return _skillSet[ESkill.Penalty]; }
            set { _skillSet[ESkill.Penalty] = value; }
        }

        public int Stamina
        {
            get { return _skillSet[ESkill.Stamina]; }
            set { _skillSet[ESkill.Stamina] = value; }
        }

        private readonly Dictionary<ESkill, int> _skillSet;
    }
}