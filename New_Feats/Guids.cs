// DO NOT change any value once committed — save compatibility depends on stable GUIDs.
namespace AttributeFeats.New_Feats
{
    public static class Guids
    {
        // EXISTING Main feats — DO NOT CHANGE (save compatibility)
        public const string str_main_to_everything = "a4c66462-a423-4a2f-8b26-770ea03d2ce0";
        public const string dex_main_to_everything = "0963babc-0579-4bb3-a33a-23949b47e68b";
        public const string con_main_to_everything = "52506f39-5c40-4780-b677-68336c44dcaa";
        public const string int_main_to_everything = "df0cb753-5704-42a0-bad0-627757af281f";
        public const string wis_main_to_everything = "41584589-3703-41c8-9a51-809805c921ad";
        public const string cha_main_to_everything = "e16525c7-ec29-4904-a69c-8b35f0c60a95";

        // NEW — do not change values once committed
        public static class Specialized
        {
            public static class Defensive
            {
                public const string Str = "2d9033de-faf8-4088-96b0-e8e28df79233";
                public const string Dex = "9333d8d4-497a-4ed1-bfb2-321efaae9741";
                public const string Con = "2b4b7a71-e694-4708-997d-a96bc390802b";
                public const string Int = "d80323de-9919-4d5f-8996-33bec924a61a";
                public const string Wis = "0cfb8868-2f5b-4e84-8113-dca1b7dc7e1f";
                public const string Cha = "0546a1ca-936b-466c-9f75-04f799a35c2e";
            }

            public static class Maneuver
            {
                public const string Str = "8c13097b-b22b-4a99-82b0-83b1b06eeba2";
                public const string Dex = "8b6c4410-00a4-4926-8703-22b664868db4";
                public const string Con = "cc0f6445-9bb9-4c52-9fce-2b73b9bd098c";
                public const string Int = "9796b63c-be1b-47dd-89ab-14dc63ec4b99";
                public const string Wis = "612b166a-5099-4477-b74c-5a07530947df";
                public const string Cha = "68534256-92db-41f1-b391-03c45e1bdc1a";
            }

            public static class Skilled
            {
                public const string Str = "bae75019-4b81-41f6-a5ef-7d948e1994ca";
                public const string Dex = "249b46c1-5afe-436e-9418-afcbabc7f229";
                public const string Con = "856026e2-5bea-4322-974b-9d64f1ad3aa2";
                public const string Int = "95405c22-71a8-40dd-b4a0-92d64b981836";
                public const string Wis = "512e3a70-697d-4a7b-9148-58ba079abc30";
                public const string Cha = "f0918636-a1f3-44fa-b95f-829737730ef5";
            }

            public static class Arcane
            {
                public const string Str = "4e8223b9-3f80-4868-965f-763341d64e13";
                public const string Dex = "abcf29a9-500d-41d3-bc0b-ff57f9840fa6";
                public const string Con = "fdc93d56-6202-4d6b-965b-99065b64e694";
                public const string Int = "3d8a271f-9224-4c6f-bb49-85169ee793fe";
                public const string Wis = "4d637462-b043-4548-833e-6366e7fa6b35";
                public const string Cha = "64517abe-bdd4-46d4-851f-fdeb11daa4a3";
            }
        }

        public static class Stance
        {
            public static class Feature
            {
                public const string Str = "f74543d5-6666-469f-a205-c73599454d64";
                public const string Dex = "48b81512-e4d7-4127-a091-1267362b7312";
                public const string Con = "0e8895ff-d33c-40bd-9797-52a4a84193a5";
                public const string Int = "9b44b583-bd8b-48d0-81b5-3df913f98a80";
                public const string Wis = "53ec3d14-00f9-4d6a-af2e-fd8aa6a77ea6";
                public const string Cha = "e1192d59-6597-49c0-8311-d99490107c93";
            }

            public static class Buff
            {
                public const string Str = "6d631775-a6cc-4217-91c6-7e90c7392e58";
                public const string Dex = "330d5c25-e564-48c9-84b5-ad2bbc4cc94d";
                public const string Con = "a5171b12-4346-40f0-b860-e3f7f0306fd3";
                public const string Int = "e84e92be-9f27-40c4-8ec3-91a018d79116";
                public const string Wis = "4fc0139c-e444-4221-9eca-8048202c11bf";
                public const string Cha = "ad9aff77-da9b-4eb6-b094-e0e9ed3a55f5";
            }

            public static class Activatable
            {
                public const string Str = "dd674856-215d-40d2-9cbc-be2fba4d8f13";
                public const string Dex = "d30242ab-cf40-46bb-ac29-456d27d798a6";
                public const string Con = "c638a34c-e9ac-484a-8853-dd85c0a28d18";
                public const string Int = "a88b3133-7f17-4a06-9227-c106cc397ef9";
                public const string Wis = "0386566e-3fff-4010-b8a8-34d55faba381";
                public const string Cha = "da9409aa-e360-4aef-964e-0b700968a6a9";
            }

            public static class AllyBuff
            {
                public const string CommandingPresence = "5e2e946d-7572-40ac-b901-8f347e3fee5a";
            }

            public static class AreaEffect
            {
                public const string CommandingPresence = "ed66e352-8aee-49e6-8ec2-bb7bba386fee";
            }
        }

        public static class Conditional
        {
            public const string FirstBlood = "102a4418-b8ee-46b9-830d-c66de0dc8ec4";
            public const string EndlessResolve = "23d76b96-2748-4e7b-9bb7-ffb48a00aea4";
            public const string Vendetta = "b4d54a88-e514-4291-9a56-01b66d57ab86";
            public const string PatientHunter = "224ec628-3085-4fdc-aa1f-9c26cf03fe41";

            public static class TriggerBuff
            {
                public const string FirstBlood = "30f90f5c-91b5-4794-b47a-031bc647c48b";
                public const string EndlessResolve = "fd56886a-08aa-4088-86cf-75628422f950";
                public const string Vendetta = "77ce625c-6936-4f84-97ef-396b2dc6260a";
                public const string PatientHunter = "c64eb9ab-c904-4252-9c89-4d13ae1327fb";
            }
        }

        public static class Conditional2 // Berserker's Last Stand, Tactical Reading
        {
            public const string BerserkersLastStand = "eeff8a1f-d105-4aca-9901-b8ca5630f38f";
            public const string TacticalReading = "76b84599-7b1d-44f2-a5f3-fff3e7c65999";

            public static class TriggerBuff
            {
                public const string BerserkersLastStand = "054d1cf0-8118-43ef-acd6-e65c57f36006";
                public const string TacticalReading = "f55c6d0e-87e2-4924-9cbd-ab1114829934";
            }
        }

        public static class Replacement
        {
            public static class WeaponInsight
            {
                public const string Str = "791445d4-206b-415b-b19a-4720440a221a";
                public const string Dex = "16886460-a104-4f66-b6f8-d2779b4e21ca";
                public const string Con = "184c9a80-6a65-4bdc-a312-7a52d6736469";
                public const string Int = "34b8fa42-4923-4c30-ae70-1bc0398b6399";
                public const string Wis = "704d20f1-1ff1-4d9a-9ac6-e4a35bc7f8cd";
                public const string Cha = "c8edba26-a2ca-4137-84ec-ed850295ca50";
            }

            public static class Extended
            {
                public const string InnerSentinel = "b2357477-c2ba-4e32-ad5e-ca56c9a6df4d";
                public const string CalculatedGrip = "07ca886f-fe82-4baa-a84e-495f338b78d2";
                public const string UnyieldingWill = "9c1fb2c2-8732-4348-88c1-f6bfe544dff0";
            }
        }

        public static class ExtendedReplacement2 // 3 more attribute-themed
        {
            public const string BrutalDefender = "e8eb1eba-d6e8-4f55-862f-5485457b5699"; // Str→CMD
            public const string LightfootDefense = "6cfe15bc-b05d-40eb-9434-a9b48d69fcb3"; // Dex→AC unarmored
            public const string IronEndurance = "8254961c-b81a-48ac-9202-e470018d112c"; // Con→HP/level
        }

        public static class ReactiveArmor
        {
            public const string SpikedDefense = "3cca56af-65d0-4854-9bef-8a7b8f44ea89";
            public const string BulwarkOfSteel = "8c842031-fca2-49f0-82c0-2120997c9735";
            public const string BulwarkOfSteelBuff = "513e5225-9e57-4411-8209-9ae948c68b7b";
        }

        public static class Derived
        {
            public const string ArcaneAegis = "b5d28e7e-a229-4c56-ba31-f99fc50ddb5b";
            public const string MartialInsight = "b14dc952-67bc-4943-9ee5-e053ed72d769";
            public const string SkilledDefender = "078125c3-80b2-429a-884f-e98443da7bbf";
            public const string MysticVitality = "f0097731-7349-4dab-ba39-30ed71181e2d";
            public const string SoulBulwark = "0a28d92a-2d2a-4a30-a589-fdb6b93a9e4f";
            public const string SoulBulwarkBuff = "a07d2617-5172-4ff0-b4b8-d3a1441eed98";
            public const string SwordSaint = "0c65b291-ce0a-4d99-b776-b9123f0bc8be";
        }

        public static class Summon
        {
            public static class Feature
            {
                public const string BloodlineOfBeasts = "0e20a30b-710f-44f4-8430-bbff56fff0ec";
                public const string QuickenedPact = "d1055cda-6ef3-4e2c-8bca-c388db283132";
                public const string VitalPact = "2a80ce8d-349a-4bfd-acc3-02ec24232121";
                public const string TacticalBinding = "d41c5951-409d-42dd-9c07-20f87ed5fac8";
                public const string InsightfulSummons = "00a4d128-8b58-42da-b958-a5be2648f530";
                public const string MagneticCalling = "b0c36de7-3739-4c0e-93a2-53f8640c0e93";
            }

            public static class OuterBuff
            {
                public const string BloodlineOfBeasts = "c3968996-caff-4558-b916-7f100125804f";
                public const string QuickenedPact = "6a34f5d7-708c-42d4-b3e4-e6f442c5c0bd";
                public const string VitalPact = "e0322c72-e3a8-4a65-a1f7-282fe40a8d44";
                public const string TacticalBinding = "5a62801c-0179-49e8-a24f-9c6144a9dd29";
                public const string InsightfulSummons = "7603587f-7e94-46ab-bc9b-c9bafe8da6bb";
                public const string MagneticCalling = "e84cbc57-dd11-4332-95ee-2821d5cfb902";
            }

            public static class InnerBuff
            {
                public const string BloodlineOfBeasts = "658df3dd-ebb5-4ccc-8be7-b3d9e5ae36ba";
                public const string QuickenedPact = "2cee59a1-c1a8-4155-a3fb-19e636612df3";
                public const string VitalPact = "a3d197d1-4f8a-4e0b-8c98-4b712c6b7f16";
                public const string TacticalBinding = "99b12003-bb78-4e5f-bded-1e0fd3331eb7";
                public const string InsightfulSummons = "8ed68309-be0a-4fa6-af9e-b93cacb51120";
                public const string MagneticCalling = "5b1a3ba4-0090-43d0-b783-7f0cd4f166de";
            }
        }

        public static class SummonerSacrifice // Family 25
        {
            public static class Feature
            {
                public const string BodyOfMyPact = "83c8873a-40ea-43ed-b279-a56a5ebd03a7";
                public const string DoubledBond = "d442fba8-51c8-48dd-a069-1e0a183fb27f";
                public const string EmpoweredSacrifice = "c54e8935-a046-4d9d-bc8b-276db4ce0c74";
            }

            public static class OuterBuff
            {
                public const string BodyOfMyPact = "2e2be264-a177-4fd1-ae09-4b890c4b62e5";
                public const string DoubledBond = "7828a4de-12ff-4b40-9beb-9e3cc1c057ce";
                public const string EmpoweredSacrifice = "01b11574-0bd9-417c-9b06-110b3a15c16d";
            }

            public static class InnerBuff
            {
                public const string BodyOfMyPact = "8c69d379-3be9-4f4f-8b10-5ae940b4d55e";
                public const string DoubledBond = "84e3137e-a1e8-4338-a334-421f58a24a6e";
                public const string EmpoweredSacrifice = "52c404a4-f11a-476b-954f-a7d39d3ed1a4";
            }
        }

        public static class SpellTag
        {
            public static class School
            {
                public const string PureWarder = "195f83c8-087b-4fa8-80a4-29c5690ac063";
                public const string MasterCaller = "8f56cc10-f632-4b33-848e-a86676e74fc9";
                public const string SeersEdge = "a71bdde1-b1b3-462d-a909-83d8e0cbef6f";
                public const string HeartsTyrant = "1e101e48-83b6-49c2-bb96-2a732e567ff5";
                public const string Spellforge = "5ebe6887-36eb-44e4-9306-d6e6e5e28745";
                public const string Veilweaver = "0c87afdb-534a-4e55-a89a-2cd83e46171d";
                public const string DeathSpeaker = "14436457-df09-4f0e-b016-3413d96132f1";
                public const string ShapeShifter = "2e15db39-ff78-432f-bffe-656a6d310ca3";
            }

            public static class Descriptor
            {
                public const string InnerFlame = "f911da43-689d-4f5d-b444-7e33978356a0";
                public const string FrozenHeart = "0456a92c-2e46-490f-8057-e61186426e22";
                public const string StormChannel = "c740ed3e-53b3-4619-b0cd-fde8827af9dc";
                public const string EtchingMind = "8626c648-1c99-4022-ac10-ce3fe3e60341";
                public const string ResonantVoice = "f5949405-4682-4911-96ba-fe7754ba951b";
            }
        }

        public static class SpellTagDescriptor2 // 4 more descriptors
        {
            public const string EthericMind = "fc85d95a-79af-4607-9404-fd0b9f8ff117"; // Force, Int
            public const string RadiantSoul = "4abca6af-b57f-47a5-bf87-d216ad6ccbf6"; // Positive Energy, Cha
            public const string HollowHeart = "4230836e-24d4-4a2b-9b59-59787a4f220b"; // Negative Energy, Wis
            public const string SubtleTyrant = "628e7fd3-75be-4b67-b785-c4e0bc6247ea"; // Mind-Affecting, Cha
        }

        public static class PolearmMaster // Family 12
        {
            public static class Feature
            {
                public const string PolearmMaster = "85389c3d-b584-4cdf-b12a-cac0fd766796";
            }
        }

        public static class DistanceDamage // Family 24
        {
            public const string AggressorsEdge = "2d2024c2-4944-4036-9649-a1f6702ca084";
            public const string MarksmansFocus = "499dd915-5ee9-4d3b-acf8-484e401d9835";
            public const string OptimalRange = "f05016c7-5126-4053-bc6b-0283d9b28eca";

            public static class Buff
            {
                public const string FlatBonus = "96c9d080-65e6-4339-9019-d0fa3c63ae48";
            }
        }
    }
}
